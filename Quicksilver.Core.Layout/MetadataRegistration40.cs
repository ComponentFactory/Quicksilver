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
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using ComponentFactory.Quicksilver.Layout;

// Visual Studio 2010 and Blend 3 look for this meta data attribute to pick up design time information
[assembly: ProvideMetadata(typeof(ComponentFactory.Quicksilver.Layout.Design.MetadataRegistration))]

namespace ComponentFactory.Quicksilver.Layout.Design
{
    /// <summary>
    /// Registration of metadata for the Quicksilver.Layout classes.
    /// </summary>
    public class MetadataRegistration : IProvideAttributeTable
    {
        #region Public
        /// <summary>
        /// Return the attribute table that is applied to design time.
        /// </summary>
        public AttributeTable AttributeTable 
        {
            get
            {
                AttributeTableBuilder builder = new AttributeTableBuilder();
                LayoutMetadata.AddAttributes(builder);
                return builder.CreateTable();
            }
        }
        #endregion
    }
}
