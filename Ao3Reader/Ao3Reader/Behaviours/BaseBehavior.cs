using System;
using Xamarin.Forms;

namespace Ao3Reader.Behaviours
{
    /// <summary>
    /// Represents a behavior that can be attached only to elements of type <T>
    /// </summary>
    /// <typeparam name="T">The type of the element that behavior can be attached to</typeparam>
    public abstract class BaseBehavior<T> : Behavior<T> where T : BindableObject
    {
        protected T AssociatedObject { get; set; }
        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        protected virtual void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}