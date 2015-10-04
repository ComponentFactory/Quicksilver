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

namespace ComponentFactory.Quicksilver.Layout.Design
{
    /// <summary>
    /// Registration of metadata for the Quicksilver.Layout classes.
    /// </summary>
    public class MetadataRegistration : IRegisterMetadata
    {
        #region Public
        /// <summary>
        /// Return the attribute table that is applied to design time.
        /// </summary>
        public void Register()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            LayoutMetadata.AddAttributes(builder);
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
        #endregion
    }

    /// <summary>
    /// Dummy implementation of attribute so MWD 4.0 attribute code still works for MWD 3.5
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ToolboxCategoryAttribute : Attribute
    {
        #region Instance Fields
        private string _categoryPath;
        private bool _alwaysShow;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ToolboxCategoryAttribute class.
        /// </summary>
        /// <param name="categoryPath">Category name.</param>
        public ToolboxCategoryAttribute(string categoryPath)
            : this(categoryPath, false)
        {
        }

        /// <summary>
        /// Initialize a new instance of the ToolboxCategoryAttribute class.
        /// </summary>
        /// <param name="categoryPath">Category name.</param>
        /// <param name="alwaysShows">Always show in this category.</param>
        public ToolboxCategoryAttribute(string categoryPath, bool alwaysShows)
        {
            _categoryPath = categoryPath;
            _alwaysShow = alwaysShows;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets the value of the always shows.
        /// </summary>
        public bool AlwaysShows 
        {
            get { return _alwaysShow; }
        }

        /// <summary>
        /// Gets the value of the category path.
        /// </summary>
        public string CategoryPath 
        {
            get { return _categoryPath; }
        }
        #endregion
    }

    /// <summary>
    /// Dummy implementation of attribute so MWD 4.0 attribute code still works for MWD 3.5
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlternateContentPropertyAttribute : Attribute
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the AlternateContentPropertyAttribute class.
        /// </summary>
        public AlternateContentPropertyAttribute()
        {
        }
        #endregion
    }
}
