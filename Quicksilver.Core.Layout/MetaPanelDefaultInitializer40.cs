// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2009. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.ComponentModel;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using ComponentFactory.Quicksilver.Layout;

namespace ComponentFactory.Quicksilver.Layout.Design
{
    /// <summary>
    /// Default initializer for the MetaPanel
    /// </summary>
    public class MetaPanelDefaultInitializer : DefaultInitializer
    {
        /// <summary>
        /// Sets default initialization for the MetaPanel. 
        /// </summary>
        /// <param name="item">Reference to MetaPanel item.</param>
        public override void InitializeDefaults(ModelItem item)
        {
            // Default to a wrap layout
            item.Properties["LayoutDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(WrapLayout)));
            
            // Default to full animation fo child elements
            item.Properties["AnimateDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(NewPositionAnimate)));
            item.Properties["AnimateDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(NewOpacityAnimate)));
            item.Properties["AnimateDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(MovePositionAnimate)));
            item.Properties["AnimateDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(RemovePositionAnimate)));
            item.Properties["AnimateDefinitions"].Collection.Add(ModelFactory.CreateItem(item.Context, typeof(RemoveOpacityAnimate)));
        }
    }
}
