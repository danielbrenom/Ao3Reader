using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ao3Reader.Behaviours
{
    public class EventToCommand : BaseBehavior<View>
    {
        private Delegate _handler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommand), null, propertyChanged: AoMudarNomeEvento);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommand));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommand));
        public static readonly BindableProperty InputConverterProperty =BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(EventToCommand));

        public string EventName
        {
            get => (string) GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter) GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            UnregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return;

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(nome);
            if (eventInfo == null)
            {
                throw new ArgumentException($"EventoParaComandoComportamento: Não pode registrar evento '{EventName}'.");
            }

            var methodInfo = typeof(EventToCommand).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent));
            _handler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, _handler);
        }

        private void UnregisterEvent(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (_handler == null)
            {
                return;
            }

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommand: Can't find any event named '{name}' on attached type");
            }

            eventInfo.RemoveEventHandler(AssociatedObject, _handler);
            _handler = null;
        }

        protected virtual void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
                return;

            object resolvedParameter;
            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                Command.Execute(resolvedParameter);
            }
        }

        private static void AoMudarNomeEvento(BindableObject bindable, object velho, object novo)
        {
            var behavior = (EventToCommand) bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            var nomeEventoVelho = (string) velho;
            var nomeEventoNovo = (string) novo;

            behavior.UnregisterEvent(nomeEventoVelho);
            behavior.RegisterEvent(nomeEventoNovo);
        }
    }
}