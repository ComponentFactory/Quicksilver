// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, PO Box 1504, 
//  Glen Waverley, Vic 3150, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Base class for extending the MultiBinding by decorating an internal instance.
    /// </summary>
    [ContentProperty("Bindings")]
    public abstract class MultiBindingDecorator : MarkupExtension
    {
        #region Instance Fields
        private MultiBinding _multiBinding;
        #endregion

        #region Identity
        /// <summary>
        /// Initalize a new instance of the MultiBindingDecorator class.
        /// </summary>
        public MultiBindingDecorator()
        {
            _multiBinding = new MultiBinding();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets the name of the BindingGroup to which this binding belongs.
        /// </summary>
        public string BindingGroupName 
        {
            get { return MultiBinding.BindingGroupName; }
            set { MultiBinding.BindingGroupName = value; }
        }

        /// <summary>
        /// Gets or sets the value to use when the binding is unable to return a value.
        /// </summary>
        public object FallbackValue
        {
            get { return MultiBinding.FallbackValue; }
            set { MultiBinding.FallbackValue = value; }
        }

        /// <summary>
        /// Returns a value that indicates whether serialization processes should serialize the effective value of the FallbackValue property on instances of this class.
        /// </summary>
        /// <returns>true if the FallbackValue property value should be serialized; otherwise, false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFallbackValue()
        {
            return MultiBinding.ShouldSerializeFallbackValue();
        }

        /// <summary>
        /// Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.
        /// </summary>
        public string StringFormat
        {
            get { return MultiBinding.StringFormat; }
            set { MultiBinding.StringFormat = value; }
        }

        /// <summary>
        /// Gets or sets the value that is used in the target when the value of the source is null.
        /// </summary>
        public object TargetNullValue
        {
            get { return MultiBinding.TargetNullValue; }
            set { MultiBinding.TargetNullValue = value; }
        }

        /// <summary>
        /// Returns a value that indicates whether the TargetNullValue property should be serialized.
        /// </summary>
        /// <returns>true if the TargetNullValue property should be serialized; otherwise, false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTargetNullValue()
        {
            return MultiBinding.ShouldSerializeTargetNullValue();
        }

        /// <summary>
        /// Gets the collection of Binding objects within this binding instance. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<BindingBase> Bindings 
        {
            get { return MultiBinding.Bindings; } 
        }

        /// <summary>
        /// Returns a value that indicates whether serialization processes should serialize the effective value of the Bindings property on instances of this class.
        /// </summary>
        /// <returns>true if the Bindings property value should be serialized; otherwise, false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBindings()
        {
            return MultiBinding.ShouldSerializeBindings();
        }

        /// <summary>
        /// Gets or sets a value that indicates the direction of the data flow of this binding.
        /// </summary>
        [DefaultValue(typeof(BindingMode), "Default")]
        public BindingMode Mode 
        {
            get { return _multiBinding.Mode; }
            set { _multiBinding.Mode = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the SourceUpdated event when a value is transferred from the binding target to the binding source.
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated 
        {
            get { return MultiBinding.NotifyOnSourceUpdated; }
            set { MultiBinding.NotifyOnSourceUpdated = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the TargetUpdated event when a value is transferred from the binding source to the binding target.
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated 
        {
            get { return MultiBinding.NotifyOnTargetUpdated; }
            set { MultiBinding.NotifyOnTargetUpdated = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the Error attached event on the bound element. 
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnValidationError 
        {
            get { return MultiBinding.NotifyOnValidationError; }
            set { MultiBinding.NotifyOnValidationError = value; }
        }

        /// <summary>
        /// Gets or sets a value that determines the timing of binding source updates.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilterCallback
        {
            get { return MultiBinding.UpdateSourceExceptionFilter; }
            set { MultiBinding.UpdateSourceExceptionFilter = value; }
        }

        /// <summary>
        /// Gets or sets a value that determines the timing of binding source updates.
        /// </summary>
        [DefaultValue(typeof(UpdateSourceTrigger), "PropertyChanged")]
        public UpdateSourceTrigger UpdateSourceTrigger 
        {
            get { return MultiBinding.UpdateSourceTrigger; }
            set { MultiBinding.UpdateSourceTrigger = value; }
        }
        
        /// <summary>
        /// Gets or sets a value that indicates whether to include the DataErrorValidationRule.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnDataErrors 
        {
            get { return MultiBinding.ValidatesOnDataErrors; }
            set { MultiBinding.ValidatesOnDataErrors = value; }
        }
        
        /// <summary>
        /// Gets or sets a value that indicates whether to include the ExceptionValidationRule.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnExceptions 
        {
            get { return MultiBinding.ValidatesOnExceptions; }
            set { MultiBinding.ValidatesOnExceptions = value; }
        }

        /// <summary>
        /// Gets the collection of ValidationRule objects for this instance of binding.
        /// </summary>
        public Collection<ValidationRule> ValidationRules 
        {
            get { return MultiBinding.ValidationRules; }
        }

        /// <summary>
        /// Indicates whether the ValidationRules property should be persisted.
        /// </summary>
        /// <returns>true if the property value has changed from its default; otherwise, false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeValidationRules()
        {
            return MultiBinding.ShouldSerializeValidationRules();
        }

        /// <summary>
        /// Returns an object that should be set on the property where this binding and extension are applied.
        /// </summary>
        /// <param name="serviceProvider">The object that can provide services for the markup extension.</param>
        /// <returns>The value to set on the binding target property.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Use the internal multi binding that we are decorating
            return MultiBinding.ProvideValue(serviceProvider);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets access to the internal multi binding instance.
        /// </summary>
        protected MultiBinding MultiBinding
        {
            get { return _multiBinding; }
        }
        #endregion
    }
}
