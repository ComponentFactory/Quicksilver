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
using System.Reflection;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Evaluates an expression using the collection of Bindings as a source of values.
    /// </summary>
    public class EvalBinding : MultiBindingDecorator
    {
        #region Static Fields
        private static MethodInfo _miCheckSealed;
        #endregion
        
        #region Instance Fields
        private string _eval;
        private Language _language;
        #endregion

        #region Identity
        static EvalBinding()
        {
            // Cache access to the internal 'CheckSealed' method
            _miCheckSealed = typeof(BindingBase).GetMethod("CheckSealed",
                                                            BindingFlags.Instance |
                                                            BindingFlags.NonPublic);
        }

        /// <summary>
        /// Initalize a new instance of the EvalBinding class.
        /// </summary>
        public EvalBinding()
        {
            // Default fields
            _eval = string.Empty;
            _language = Language.CSharp;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the expression to evaluate.
        /// </summary>
        public string Eval 
        {
            get { return _eval; }

            set
            {
                // Cannot update the Eval if the binding instance is sealed
                _miCheckSealed.Invoke(MultiBinding, null);
                _eval = value;
            }
        }

        /// <summary>
        /// Gets and sets the expression to evaluate.
        /// </summary>
        public Language Language
        {
            get { return _language; }

            set
            {
                // Cannot update the Language if the binding instance is sealed
                _miCheckSealed.Invoke(MultiBinding, null);
                _language = value;
            }
        }

        /// <summary>
        /// Returns an object that should be set on the property where this binding and extension are applied.
        /// </summary>
        /// <param name="serviceProvider">The object that can provide services for the markup extension.</param>
        /// <returns>The value to set on the binding target property.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Our custom multi-value converter is used to evaluate the expression each time it is called
            IProvideValueTarget provideTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            MultiBinding.Converter = new EvalBindingConverter(Eval, Language, provideTarget.TargetObject as FrameworkElement);

            // Let base class return actual multi-binding instance
            return base.ProvideValue(serviceProvider);
        }
        #endregion
    }
}
