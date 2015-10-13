using System;
using System.Collections.Generic;
using System.Text;
using Neurotec.Samples;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Gui;
using System.Windows.Forms;
using System.Drawing;
using Neurotec.Gui;
using Neurotec.Images;

namespace PSCBioVerificationFace
{
	public class FaceRecord : AbstractBiometricRecord
	{
		#region Private fields

		private NLTemplate _nlTemplate;
		private NleDetectionDetails[] _detectionDetails;
		
		#endregion

		#region Public properties

		public NLTemplate Template
		{
			get
			{
				return _nlTemplate;
			}
			set
			{
				_nlTemplate = value;
			}
		}
		public NleDetectionDetails[] DetectionDetails
		{
			get
			{
				return _detectionDetails;
			}
			set
			{
				_detectionDetails = value;
			}
		}

		#endregion

		#region Public constructor
		public FaceRecord(NLTemplate nlTemplate, NImage image, params NleDetectionDetails[] detectionDetails)
		{
			this._nlTemplate = nlTemplate;
			this._detectionDetails = detectionDetails;
			this.Image = image;
		}
		#endregion

		#region Public methods
		public override void AddToTemplate(NTemplate tmpl)
		{
			if (tmpl.Faces == null)
			{
				tmpl.AddFaces();
			}

			foreach (NLRecord item in _nlTemplate.Records)
			{
				tmpl.Faces.Records.AddCopy(item);
			}
		}

		public override object CreateView(Panel hostPanel)
		{
			NLView nlView = new NLView();
			if (_image != null)
				nlView.Image = _image.ToBitmap();
			else
				nlView.Image = null;

			nlView.DetectionDetails = _detectionDetails;
			nlView.Dock = DockStyle.Fill;
			hostPanel.Controls.Add(nlView);
			hostPanel.PerformLayout();
			nlView.AutoScroll = true;
			hostPanel.Invalidate();
			return nlView;
		}

		public override string ToString()
		{
			return string.Format("Face");
		}
		#endregion

		#region IDisposable Members

		public override void Dispose()
		{
			foreach (NLRecord rec in _nlTemplate.Records)
			{
				rec.Dispose();
			}

			if (_image != null)
			{
				_image.Dispose();
				_image = null;
			}
		}

		#endregion
	}
}
