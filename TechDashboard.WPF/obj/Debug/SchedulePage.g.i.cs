﻿#pragma checksum "..\..\SchedulePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7FCBC8653F33E2CBC9518E36F32EB8CF"
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
using TechDashboard.WPF;


namespace TechDashboard.WPF {
    
    
    /// <summary>
    /// SchedulePage
    /// </summary>
    public partial class SchedulePage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 59 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid gridSchedule;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelTitle;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker filterStartDate;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker filterEndDate;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonFilter;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\SchedulePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSTFilter;
        
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
            System.Uri resourceLocater = new System.Uri("/TechDashboard.WPF;component/schedulepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SchedulePage.xaml"
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
            this.gridSchedule = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.labelTitle = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.filterStartDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.filterEndDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.buttonFilter = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\SchedulePage.xaml"
            this.buttonFilter.Click += new System.Windows.RoutedEventHandler(this.buttonFilter_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtSTFilter = ((System.Windows.Controls.TextBox)(target));
            
            #line 99 "..\..\SchedulePage.xaml"
            this.txtSTFilter.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtSTFilter_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

