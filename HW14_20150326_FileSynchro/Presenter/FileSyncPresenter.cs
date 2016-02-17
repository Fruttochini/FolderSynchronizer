using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW14_20150326_FileSynchro.Model;
using HW14_20150326_FileSynchro.View;
using System.IO;

namespace HW14_20150326_FileSynchro.Presenter
{
	class FileSyncPresenter
	{
		/// <summary>
		/// incapsulate model to be used for synchronization
		/// </summary>
		FileSyncModel _model;

		/// <summary>
		/// IView reference
		/// </summary>
		IView _view;

		/// <summary>
		/// Constructor with View parameter
		/// </summary>
		/// <param name="v"></param>
		public FileSyncPresenter (IView v)
		{
			_model = new FileSyncModel();
			_view = v;
			///Subscribe for Synchronize event with Synchronize function
			_view.Synchronize+=new EventHandler<EventArgs>(Synchronize);
		}

		/// <summary>
		/// Synchronize function
		/// Gets parameter from view and send it to Model
		/// Starts synchronization process
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Synchronize (object sender, EventArgs e)
		{
			_model.SourceFolder = new DirectoryInfo(_view.SourceFolder);
			_model.DestinationFolder = new DirectoryInfo(_view.DestinationFolder);
			try
			{
				_model.Synchronize(_view.Direction);
			}
			catch (Exception ex)
			{
				///catch exception from model and show a message in view
				_view.ShowMessage(ex.Message, "Exception");
			}
		}
	}
}
