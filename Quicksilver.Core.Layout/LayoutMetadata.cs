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
    /// Metadata for the Quicksilver.Layout set of classes.
    /// </summary>
    public class LayoutMetadata
    {
        #region Public
        /// <summary>
        /// Apply attribute metadata.
        /// </summary>
        public static void AddAttributes(AttributeTableBuilder builder) 
        {
            AddMeasureElementAttributes(builder);
            AddDockLayoutAttributes(builder);
            AddGridLayoutAttributes(builder);
            AddBaseDefinitionAttributes(builder);
            AddColumnDefinitionAttributes(builder);
            AddRowDefinitionAttributes(builder);
            AddRadialLayoutAttributes(builder);
            AddStackLayoutAttributes(builder);
            AddUniformGridLayoutAttributes(builder);
            AddWrapLayoutAttributes(builder);
            AddMovePositionAnimateAttributes(builder);
            AddNewPositionAnimateAttributes(builder);
            AddNewOpacityAnimateAttributes(builder);
            AddRemovePositionAnimateAttributes(builder);
            AddRemoveOpacityAnimateAttributes(builder);
            AddMetaPanelAttributes(builder);
        }
        #endregion

        #region Private
        private static void AddMeasureElementAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(MeasureElement),
                                b =>
                                {
                                    // MeasureElement class
                                    b.AddCustomAttributes(new ToolboxBrowsableAttribute(false));
                                    b.AddCustomAttributes(new DefaultPropertyAttribute("LayoutDefinitions"));
                                    b.AddCustomAttributes(new DefaultEventAttribute("IsAnimatingChanged"));

                                    // MeasureElement properties
                                    b.AddCustomAttributes("AllowDrop", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("BindingGroup", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("CacheMode", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Clip", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ClipToBounds", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ContextMenu", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Cursor", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("DataContext", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Effect", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Focusable", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("FocusVisualStyle", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ForceCursor", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("FlowDirection", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Height", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("HorizontalAlignment", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("InputScope", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("IsEnabled", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("IsHitTestVisible", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ItemHeight", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ItemWidth", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("LayoutTransform", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Margin", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("MaxHeight", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("MaxWidth", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("MinHeight", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("MinWidth", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Opacity", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("OpacityMask", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("OverridesDefaultStyle", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Projection", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("RenderTransform", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("RenderTransformOrigin", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Resources", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("SnapsToDevicePixels", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Style", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ToolTip", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Uid", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("UseLayoutRounding", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("VerticalRounding", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("VerticalAlignment", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Visibility", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Width", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ZIndex", new BrowsableAttribute(false));
                                });
        }

        private static void AddBaseDefinitionAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(BaseDefinition),
                                b =>
                                {
                                    // BaseDefinition properties
                                    b.AddCustomAttributes("AllowDrop", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("BindingGroup", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("CommandBindings", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ContextMenu", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Cursor", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("DataContext", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("FocusVisualStyle", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Focusable", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ForceCursor", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("InputScope", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("IsEnabled", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Language", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Name", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("OverridesDefaultStyle", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Resources", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("Style", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("ToolTip", new BrowsableAttribute(false));
                                });
        }

        private static void AddColumnDefinitionAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(ColumnDefinition),
                                b =>
                                {
                                    // ColumnDefinition properties
                                    b.AddCustomAttributes("Height", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("MinWidth", new CategoryAttribute("Column"));
                                    b.AddCustomAttributes("MinWidth", new DescriptionAttribute("Minimum width of the column"));
                                    b.AddCustomAttributes("MaxWidth", new CategoryAttribute("Column"));
                                    b.AddCustomAttributes("MaxWidth", new DescriptionAttribute("Maximum width of the column"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Column"));
                                    b.AddCustomAttributes("Width", new CategoryAttribute("Column"));
                                });
        }

        private static void AddRowDefinitionAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(RowDefinition),
                                b =>
                                {
                                    // RowDefinition properties
                                    b.AddCustomAttributes("Height", new CategoryAttribute("Row"));
                                    b.AddCustomAttributes("MinHeight", new CategoryAttribute("Row"));
                                    b.AddCustomAttributes("MinHeight", new DescriptionAttribute("Minimum height of the column"));
                                    b.AddCustomAttributes("MaxHeight", new CategoryAttribute("Row"));
                                    b.AddCustomAttributes("MaxHeight", new DescriptionAttribute("Maximum height of the column"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Row"));
                                    b.AddCustomAttributes("Width", new BrowsableAttribute(false));
                                });
        }

        private static void AddGridLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(GridLayout),
                                b =>
                                {
                                    // GridLayout properties
                                    b.AddCustomAttributes("ColumnDefinitions", new CategoryAttribute("Grid"));
                                    b.AddCustomAttributes("ColumnDefinitions", new DescriptionAttribute("Collection of grid columns"));
                                    b.AddCustomAttributes("ColumnDefinitions", new AlternateContentPropertyAttribute());
                                    b.AddCustomAttributes("RowDefinitions", new CategoryAttribute("Grid"));
                                    b.AddCustomAttributes("RowDefinitions", new DescriptionAttribute("Collection of grid rows"));
                                    b.AddCustomAttributes("RowDefinitions", new AlternateContentPropertyAttribute());
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Grid"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Grid"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddDockLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(DockLayout),
                                b =>
                                {
                                    // DockLayout properties
                                    b.AddCustomAttributes("LastChildFill", new CategoryAttribute("Dock"));
                                    b.AddCustomAttributes("LastChildFill", new DescriptionAttribute("Should last child element fill all remaining space"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Dock"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Dock"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddRadialLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(RadialLayout),
                                b =>
                                {
                                    // RadialLayout properties
                                    b.AddCustomAttributes("StartAngle", new CategoryAttribute("Radial"));
                                    b.AddCustomAttributes("StartAngle", new DescriptionAttribute("Start positioning child elements at this angle"));
                                    b.AddCustomAttributes("EndAngle", new CategoryAttribute("Radial"));
                                    b.AddCustomAttributes("EndAngle", new DescriptionAttribute("End positioning child elements at this angle"));
                                    b.AddCustomAttributes("Circle", new CategoryAttribute("Radial"));
                                    b.AddCustomAttributes("Circle", new DescriptionAttribute("Should last child element fill all remaining space"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Radial"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Radial"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddStackLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(StackLayout),
                                b =>
                                {
                                    // StackLayout properties
                                    b.AddCustomAttributes("Orientation", new CategoryAttribute("Stack"));
                                    b.AddCustomAttributes("Orientation", new DescriptionAttribute("Direction in which child elements are arranged"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Stack"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Stack"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddUniformGridLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(UniformGridLayout),
                                b =>
                                {
                                    // UniformGridLayout properties
                                    b.AddCustomAttributes("Rows", new CategoryAttribute("UniformGrid"));
                                    b.AddCustomAttributes("Rows", new DescriptionAttribute("Number of rows in grid"));
                                    b.AddCustomAttributes("Columns", new CategoryAttribute("UniformGrid"));
                                    b.AddCustomAttributes("Columns", new DescriptionAttribute("Number of columns in grid"));
                                    b.AddCustomAttributes("FirstColumn", new CategoryAttribute("UniformGrid"));
                                    b.AddCustomAttributes("FirstColumn", new DescriptionAttribute("Number of leading blank cells in first row of grid"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("UniformGrid"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("UniformGrid"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddWrapLayoutAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(WrapLayout),
                                b =>
                                {
                                    // WrapLayout properties
                                    b.AddCustomAttributes("ItemWidth", new CategoryAttribute("Wrap"));
                                    b.AddCustomAttributes("ItemWidth", new DescriptionAttribute("Width of all child elements"));
                                    b.AddCustomAttributes("ItemHeight", new CategoryAttribute("Wrap"));
                                    b.AddCustomAttributes("ItemHeight", new DescriptionAttribute("Height of all child elements"));
                                    b.AddCustomAttributes("Orientation", new CategoryAttribute("Wrap"));
                                    b.AddCustomAttributes("Orientation", new DescriptionAttribute("Direction in which child elements are arranged"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Wrap"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for layout instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Wrap"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddMovePositionAnimateAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(MovePositionAnimate),
                                b =>
                                {
                                    // EasingAnimate properties
                                    b.AddCustomAttributes("Duration", new CategoryAttribute("Move Position"));
                                    b.AddCustomAttributes("Duration", new DescriptionAttribute("Easing algorithm is applied over this duration"));
                                    b.AddCustomAttributes("Easing", new CategoryAttribute("Move Position"));
                                    b.AddCustomAttributes("Easing", new DescriptionAttribute("Algorithm used to get from start to end values"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Move Position"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for animate instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Move Position"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                });
        }

        private static void AddNewPositionAnimateAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(NewPositionAnimate),
                                b =>
                                {
                                    // NewPositionAnimate properties
                                    b.AddCustomAttributes("Location", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Location", new DescriptionAttribute("Starting location for a child element being added"));
                                    b.AddCustomAttributes("Size", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Size", new DescriptionAttribute("Starting size for a child element being added"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for animate instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                    b.AddCustomAttributes("Duration", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Duration", new DescriptionAttribute("Easing algorithm is applied over this duration"));
                                    b.AddCustomAttributes("Easing", new CategoryAttribute("New Position"));
                                    b.AddCustomAttributes("Easing", new DescriptionAttribute("Algorithm used to get from start to end values"));
                                });
        }

        private static void AddRemovePositionAnimateAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(RemovePositionAnimate),
                                b =>
                                {
                                    // RemovePositionAnimate properties
                                    b.AddCustomAttributes("Location", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Location", new DescriptionAttribute("Ending location for a child element being added"));
                                    b.AddCustomAttributes("Size", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Size", new DescriptionAttribute("Ending size for a child element being added"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for animate instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                    b.AddCustomAttributes("Duration", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Duration", new DescriptionAttribute("Easing algorithm is applied over this duration"));
                                    b.AddCustomAttributes("Easing", new CategoryAttribute("Remove Position"));
                                    b.AddCustomAttributes("Easing", new DescriptionAttribute("Algorithm used to get from start to end values"));
                                });
        }

        private static void AddNewOpacityAnimateAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(NewOpacityAnimate),
                                b =>
                                {
                                    // NewOpacityAnimate properties
                                    b.AddCustomAttributes("Start", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("Start", new DescriptionAttribute("Starting opacity for a child element being added"));
                                    b.AddCustomAttributes("End", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("End", new DescriptionAttribute("Ending opacity when child element has finished being added"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for animate instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                    b.AddCustomAttributes("Duration", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("Duration", new DescriptionAttribute("Easing algorithm is applied over this duration"));
                                    b.AddCustomAttributes("Easing", new CategoryAttribute("New Opacity"));
                                    b.AddCustomAttributes("Easing", new DescriptionAttribute("Algorithm used to get from start to end values"));
                                });
        }

        private static void AddRemoveOpacityAnimateAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(RemoveOpacityAnimate),
                                b =>
                                {
                                    // RemoveOpacityAnimate properties
                                    b.AddCustomAttributes("Start", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("Start", new DescriptionAttribute("Starting opacity for a child element being removed"));
                                    b.AddCustomAttributes("End", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("End", new DescriptionAttribute("Ending opacity when child element has finished being removed"));
                                    b.AddCustomAttributes("Id", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("Id", new DescriptionAttribute("Identifier for animate instance"));
                                    b.AddCustomAttributes("Tag", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("Tag", new EditorBrowsableAttribute(EditorBrowsableState.Always));
                                    b.AddCustomAttributes("Duration", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("Duration", new DescriptionAttribute("Easing algorithm is applied over this duration"));
                                    b.AddCustomAttributes("Easing", new CategoryAttribute("Remove Opacity"));
                                    b.AddCustomAttributes("Easing", new DescriptionAttribute("Algorithm used to get from start to end values"));
                                });
        }

        private static void AddMetaPanelAttributes(AttributeTableBuilder builder)
        {
            builder.AddCallback(typeof(MetaPanel), 
                                b => 
                                {
                                    // MetaPanel class
                                    b.AddCustomAttributes(new FeatureAttribute(typeof(MetaPanelDefaultInitializer)));
                                    b.AddCustomAttributes(new ToolboxCategoryAttribute("Panels", true));
                                    b.AddCustomAttributes(new DescriptionAttribute("Panel that has polymorphic layout/animate strategies."));

                                    // MetaPanel properties
                                    b.AddCustomAttributes("AnimateDefinitions", new CategoryAttribute("MetaPanel"));
                                    b.AddCustomAttributes("AnimateDefinitions", new DescriptionAttribute("Collection of animate definitions"));
                                    b.AddCustomAttributes("AnimateDefinitions", new PropertyOrderAttribute(PropertyOrder.Early));
                                    b.AddCustomAttributes("AnimateDefinitions", new AlternateContentPropertyAttribute());
                                    b.AddCustomAttributes("AnimateDefinitions", new NewItemTypesAttribute(typeof(NewOpacityAnimate), typeof(NewPositionAnimate), typeof(MovePositionAnimate), typeof(RemoveOpacityAnimate), typeof(RemovePositionAnimate)));
                                    b.AddCustomAttributes("AnimateId", new CategoryAttribute("MetaPanel"));
                                    b.AddCustomAttributes("AnimateId", new DescriptionAttribute("Identifier of animate definitions to apply"));
                                    b.AddCustomAttributes("ClipToBounds", new CategoryAttribute("Appearance"));
                                    b.AddCustomAttributes("ClipToBounds", new DescriptionAttribute("Clip child elements to panel area"));
                                    b.AddCustomAttributes("ClipToBounds", new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                                    b.AddCustomAttributes("DisableOnElementCount", new CategoryAttribute("MetaPanel"));
                                    b.AddCustomAttributes("DisableOnElementCount", new DescriptionAttribute("Disable animation when child element count exceeds value"));
                                    b.AddCustomAttributes("DisableOnElementCount", new PropertyOrderAttribute(PropertyOrder.Late));
                                    b.AddCustomAttributes("IsAnimating", new BrowsableAttribute(false));
                                    b.AddCustomAttributes("LayoutDefinitions", new CategoryAttribute("MetaPanel"));
                                    b.AddCustomAttributes("LayoutDefinitions", new DescriptionAttribute("Collection of Layout Settingss"));
                                    b.AddCustomAttributes("LayoutDefinitions", new PropertyOrderAttribute(PropertyOrder.Early));
                                    b.AddCustomAttributes("LayoutDefinitions", new AlternateContentPropertyAttribute());
                                    b.AddCustomAttributes("LayoutDefinitions", new NewItemTypesAttribute(typeof(CanvasLayout), typeof(DockLayout), typeof(GridLayout), typeof(RadialLayout), typeof(StackLayout), typeof(StretchLayout), typeof(UniformGridLayout), typeof(WrapLayout)));
                                    b.AddCustomAttributes("LayoutId", new CategoryAttribute("MetaPanel"));
                                    b.AddCustomAttributes("LayoutId", new DescriptionAttribute("Identifier of Layout Settingss to apply"));
                                });
        }
        #endregion
    }
}
