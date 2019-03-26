using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Connected.CredentialStorage;
using Microsoft.VisualStudio.Shell.Interop;
using VSIXEnt.Helper;
using VSIXEnt.Services;
using VSIXEnt.UI;
using Task = System.Threading.Tasks.Task;

namespace VSIXEnt
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ErrorSolver
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        /// 
        
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("9d2d83ac-9900-4cbd-b558-e5307c0d205e");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorSolver"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ErrorSolver(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
          
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ErrorSolver Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Verify the current thread is the UI thread - the call to AddCommand in ErrorSolver's constructor requires
            // the UI thread.
            ThreadHelper.ThrowIfNotOnUIThread();

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new ErrorSolver(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        /// 

        string exceptionString = "";
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
           bool isUserLogedIn= Authnticator.isUserLogedIn();
            if(!isUserLogedIn)
            {
                Login login = new Login();
                login.Show();
            }
            else
            {
                bool isExceptionThrown = ExceptionFinder.isExceptionThrown();
                if (!isExceptionThrown)
                {
                    ErrorService errorService = new ErrorService();
                    ErrorListProvider errorListProvider = new ErrorListProvider(errorService);
                    IEnumerable<IVsTaskItem> list = VsShellUtilities.GetErrorListItems(errorService);
                    int numberOfErrors = 0;

                    List<string> errorMessages = new List<string>();
                    errorMessages.Clear();
                    foreach (var item in list)
                    {
                        string errortext;
                        item.get_Text(out errortext);
                        errorMessages.Add(errortext);
                        numberOfErrors++;
                    }
                    if(numberOfErrors==0)
                    {
                        MessageBox.Show("Everything is OK... You are good to Go.");
                    }
                    else
                    {
                        foreach (var item in errorMessages)
                        {
                            MessageBox.Show(item);
                        }
                    }
                    
                    
                }
                else if (isExceptionThrown)
                {
                    DTE2 dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
                    string filename = dte.FileName;
                    Solution solution = dte.Solution;
                    string fileName = dte.ActiveDocument.FullName;
                    EnvExtractor envExtractor = new EnvExtractor();
                    string extionName = envExtractor.detectEnvironoment(fileName);
                    EnvironomentSpecifier environomentSpecifier = new EnvironomentSpecifier();
                 string envName=   environomentSpecifier.getEnvNameByFileExtensionName(extionName);
                    OutputWindowPane outputWindowPane = dte.ToolWindows.OutputWindow.OutputWindowPanes.Item("Debug");
                    outputWindowPane.TextDocument.Selection.SmartFormat();

                    outputWindowPane.TextDocument.Selection.SelectAll();

                    string newText = outputWindowPane.TextDocument.Selection.Text;

                    string[] buildOrder = newText.Split('\n');

                    int index = 0;
                    int count = 0;

                    foreach (var item in buildOrder)
                    {
                        if (item.Contains("An unhandled"))
                        {
                            index = count;
                        }
                        count++;
                    }
                    exceptionString = buildOrder[index + 1];
                    // process new output in newText
                    exceptionString = exceptionString + " in " + envName;
                    //MessageBox.Show(exceptionString);
                    Home errorDisplay = new Home(exceptionString);
                    errorDisplay.ShowDialog();
                }


            }




           
        }
    }
}
