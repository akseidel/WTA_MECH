﻿#pragma checksum "..\..\FormToolSetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6C40D315FD6B27BAF420B68B683F3DA8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WTA_MECH;


namespace WTA_MECH {
    
    
    /// <summary>
    /// FormToolSetting
    /// </summary>
    public partial class FormToolSetting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Body;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel A1;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_close;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel A2;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MsgLabelTop;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MsgLabelBot;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MsgTextBlockMainMsg;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid SettingsGrid;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox RootSearchPath;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkTagOtherViews;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\FormToolSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ResetToDefaults;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WTA_MECH;component/formtoolsetting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FormToolSetting.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 13 "..\..\FormToolSetting.xaml"
            ((WTA_MECH.FormToolSetting)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.DragWindow);
            
            #line default
            #line hidden
            
            #line 14 "..\..\FormToolSetting.xaml"
            ((WTA_MECH.FormToolSetting)(target)).LocationChanged += new System.EventHandler(this.Window_LocationChanged);
            
            #line default
            #line hidden
            
            #line 17 "..\..\FormToolSetting.xaml"
            ((WTA_MECH.FormToolSetting)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 18 "..\..\FormToolSetting.xaml"
            ((WTA_MECH.FormToolSetting)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Body = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.A1 = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            this.btn_close = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.A2 = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 6:
            this.MsgLabelTop = ((System.Windows.Controls.TextBlock)(target));
            
            #line 59 "..\..\FormToolSetting.xaml"
            this.MsgLabelTop.MouseEnter += new System.Windows.Input.MouseEventHandler(this.MsgLabelTop_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 7:
            this.MsgLabelBot = ((System.Windows.Controls.TextBlock)(target));
            
            #line 68 "..\..\FormToolSetting.xaml"
            this.MsgLabelBot.MouseEnter += new System.Windows.Input.MouseEventHandler(this.MsgLabelBot_MouseEnter);
            
            #line default
            #line hidden
            
            #line 68 "..\..\FormToolSetting.xaml"
            this.MsgLabelBot.MouseLeave += new System.Windows.Input.MouseEventHandler(this.MsgLabelBot_MouseLeave);
            
            #line default
            #line hidden
            
            #line 68 "..\..\FormToolSetting.xaml"
            this.MsgLabelBot.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.MsgLabelBot_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.MsgTextBlockMainMsg = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.SettingsGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.RootSearchPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.chkTagOtherViews = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 12:
            this.ResetToDefaults = ((System.Windows.Controls.Button)(target));
            
            #line 168 "..\..\FormToolSetting.xaml"
            this.ResetToDefaults.Click += new System.Windows.RoutedEventHandler(this.ResetToDefaults_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

