using System.Windows;
using System.Windows.Documents;

namespace NTouchTypeTrainer.Views
{
    public partial class BindableRichTextBox
    {
        public BindableRichTextBox()
        {
            InitializeComponent();
        }

        public FlowDocument Document
        {
            get => (FlowDocument)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument),
                typeof(BindableRichTextBox), new PropertyMetadata(OnDocumentChanged));

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bindableRichTextBox = (BindableRichTextBox)d;
            if (e.NewValue is FlowDocument document)
            {
                bindableRichTextBox.RichTextBox.Document = document;
            }
            else
            {
                bindableRichTextBox.RichTextBox.Document = new FlowDocument();
            }
        }
    }
}
