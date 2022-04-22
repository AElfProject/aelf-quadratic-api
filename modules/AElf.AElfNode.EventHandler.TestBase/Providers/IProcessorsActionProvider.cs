using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.CSharp.Core;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.TestBase.Providers
{
    public interface IProcessorsActionProvider
    {
        void RegisterProcessor();
        Func<object, EventContext, Task> GetProcessorAction<T>(Type type) where T : IEvent<T>, new();
    }

    public class ProcessorsActionProvider : IProcessorsActionProvider, ISingletonDependency
    {
        private readonly List<IAElfEventProcessor> _processors;
        private const string TargetMethodName = nameof(IAElfEventProcessor.HandleEventAsync);
        private readonly Dictionary<Type, Func<object, EventContext, Task>> _processorActionDic;

        public ProcessorsActionProvider(
            IEnumerable<IAElfEventProcessor> processors)
        {
            _processors = processors.ToList();
           _processorActionDic = new Dictionary<Type, Func<object, EventContext, Task>>();
        }

        public void RegisterProcessor()
        {
            foreach (var processor in _processors)
            {
                var targetMethodInfo = processor.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(x => x.Name == TargetMethodName && x.GetParameters().Length > 1);
                var parameter = targetMethodInfo.GetParameters().First();
                var pType = parameter.ParameterType;
                Task TargetMethodCall(object eventEto, EventContext context)
                {
                    return (Task)targetMethodInfo.Invoke(processor, new object[] {eventEto, context});
                }

                if (!_processorActionDic.ContainsKey(pType))
                {
                    _processorActionDic.Add(pType, TargetMethodCall);
                }
            }
        }

        public Func<object, EventContext, Task> GetProcessorAction<T>(Type type) where T : IEvent<T>, new()
        {
            return _processorActionDic[type];
        }
    }
}