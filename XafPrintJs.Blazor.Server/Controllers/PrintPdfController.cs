using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using Microsoft.JSInterop;
using System.Reflection;

namespace XafPrintJs.Blazor.Server.Controllers
{
    public class PrintPdfController : ViewController
    {
        SimpleAction PrintPdfAction;
        public PrintPdfController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            PrintPdfAction = new SimpleAction(this, "Print PDF", "View");
            PrintPdfAction.Execute += PrintPdfAction_Execute;
            
        }
        public static string GetEmbeddedResourceAsBase64(string resourceName)
        {
            string base64String = null;

            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Open the embedded resource as a stream
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    // Resource doesn't exist
                    return null;
                }

                // Create a byte array to hold the stream data
                byte[] bytes = new byte[stream.Length];

                // Read the bytes from the stream
                stream.Read(bytes, 0, (int)stream.Length);

                // Convert the byte array to a base64 string
                base64String = Convert.ToBase64String(bytes);
            }

            return base64String;
        }
        private async void PrintPdfAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            //Export report to memory stream https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.UI.XtraReport.ExportToPdf(System.IO.Stream-DevExpress.XtraPrinting.PdfExportOptions)
            var pdfBase64 = GetEmbeddedResourceAsBase64("XafPrintJs.Blazor.Server.sample.pdf");

            var Js = (this.Application as BlazorApplication).ServiceProvider.GetRequiredService<IJSRuntime>();
            await Js.InvokeVoidAsync("PrintPdf", pdfBase64);

            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112737/).
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
    }
}
