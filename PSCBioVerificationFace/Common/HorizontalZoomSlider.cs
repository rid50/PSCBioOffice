using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PSCBioVerificationFace.Common
{
	public partial class HorizontalZoomSlider : UserControl
	{
		#region Public constructor
		public HorizontalZoomSlider()
		{
			InitializeComponent();
		}
		#endregion

		#region Public methods / properties

		[Category("Appearance")]
		public float ZoomValue
		{
			get
			{
				return trackZoom.Value / 100f;
			}
			set
			{
				trackZoom.Value = (int)(value * 100);
			}
		}

        [Category("Appearance")]
        public string Label
        {
            get
            {
                return labelZoom.Text;
            }
            set
            {
                labelZoom.Text = value;
            }
        }

		#endregion

		#region Private methods
		private void numUpDown_ValueChanged(object sender, EventArgs e)
		{
			trackZoom.Value = (int)numUpDown.Value;
			if (ZoomValueChanged != null)
				ZoomValueChanged(this, new ZoomEventArgs(trackZoom.Value / 100f));
		}

		private void tbZoom_ValueChanged(object sender, EventArgs e)
		{
			numUpDown.Value = trackZoom.Value;
		}
		#endregion

		#region Public events
		public event EventHandler<ZoomEventArgs> ZoomValueChanged;
		#endregion

	}

	public class ZoomEventArgs : EventArgs
	{
		private float value;
		public ZoomEventArgs(float val)
		{
			value = val;
		}
		public float ZoomValue
		{
			get
			{
				return value;
			}
		}
	}
}
