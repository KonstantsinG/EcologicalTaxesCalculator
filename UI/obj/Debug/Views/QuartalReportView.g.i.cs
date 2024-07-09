﻿#pragma checksum "..\..\..\Views\QuartalReportView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1FBD099CD1A534FCF51AEAE34A8BCC5B30DF0D65A6EB90947DAD8ADD6F301807"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
using UI.Controls;
using UI.Views;


namespace UI.Views {
    
    
    /// <summary>
    /// QuartalReportView
    /// </summary>
    public partial class QuartalReportView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker datePicker;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox quartalCBox;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock resultsBlock;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UI.Controls.DataInput monthOnePanel;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UI.Controls.DataInput monthTwoPanel;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UI.Controls.DataInput monthThreePanel;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\Views\QuartalReportView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UI.Controls.DataInput resultInputPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/UI;component/views/quartalreportview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\QuartalReportView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.datePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 37 "..\..\..\Views\QuartalReportView.xaml"
            this.datePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.OnDatePickerSelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.quartalCBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 42 "..\..\..\Views\QuartalReportView.xaml"
            this.quartalCBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OnQuartalCBoxSelectionChanged);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\Views\QuartalReportView.xaml"
            this.quartalCBox.DropDownClosed += new System.EventHandler(this.OnQuartalCBoxDropDownClosed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.resultsBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 53 "..\..\..\Views\QuartalReportView.xaml"
            this.resultsBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.OnResultsBlockMouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 62 "..\..\..\Views\QuartalReportView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnSaveResultsButtonClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.monthOnePanel = ((UI.Controls.DataInput)(target));
            return;
            case 6:
            this.monthTwoPanel = ((UI.Controls.DataInput)(target));
            return;
            case 7:
            this.monthThreePanel = ((UI.Controls.DataInput)(target));
            return;
            case 8:
            this.resultInputPanel = ((UI.Controls.DataInput)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
