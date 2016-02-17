using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HW14_20150326_FileSynchro.Presenter;



namespace HW14_20150326_FileSynchro
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			///Creating an instance of our "View"
			MainWindow mw = new MainWindow();

			///Creating an instance of "Presenter" with created above View
			FileSyncPresenter fsp = new FileSyncPresenter(mw);

			///Run created view
			Application.Run(mw);
		}
	}
}
