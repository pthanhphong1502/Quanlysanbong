﻿#pragma checksum "..\..\..\..\ResourceXAML\NumberSipner.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "12EB7FD3D92B8984A35975928C02B3085000ABEBAC7953AD3C9B4FC2866BDDD8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using QlySanBong.ResourceXAML;
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


namespace QlySanBong.ResourceXAML {
    
    
    /// <summary>
    /// NumberSipner
    /// </summary>
    public partial class NumberSipner : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 74 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.TextBox tb_main;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Button cmdUp;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Button cmdDown;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brdBrush;
        
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
            System.Uri resourceLocater = new System.Uri("/QlySanBong;component/resourcexaml/numbersipner.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
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
            this.tb_main = ((System.Windows.Controls.TextBox)(target));
            
            #line 84 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
            this.tb_main.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tb_main_TextChanged);
            
            #line default
            #line hidden
            
            #line 85 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
            this.tb_main.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.tb_main_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cmdUp = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
            this.cmdUp.Click += new System.Windows.RoutedEventHandler(this.cmdUp_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmdDown = ((System.Windows.Controls.Button)(target));
            
            #line 111 "..\..\..\..\ResourceXAML\NumberSipner.xaml"
            this.cmdDown.Click += new System.Windows.RoutedEventHandler(this.cmdDown_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.brdBrush = ((System.Windows.Controls.Border)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

