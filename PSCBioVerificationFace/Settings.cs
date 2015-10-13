using System.Configuration;
using Neurotec.Samples;

namespace PSCBioVerificationFace
{
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
	internal sealed partial class Settings
	{
        
        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }
		
		public override object this [ string propertyName ] 
		{
			get 
			{ 
				if (SampleSettingsProvider.IsMonoRuntime)
				{
					SettingsPropertyValue val = PropertyValues[propertyName];
					if (val.PropertyValue == null) throw new System.NullReferenceException();
				}
				return base[propertyName];
			}
			set { base[propertyName] = value;}
		}
		
		public override SettingsPropertyCollection Properties
		{
			get
			{
				if (!SampleSettingsProvider.IsMonoRuntime)
				{
					return base.Properties;
				}
				else // bug workaround for mono
				{
					SettingsPropertyCollection properties = base.Properties;
					SampleSettingsProvider local_provider = null;
					if (Providers [SampleSettingsProvider.ProviderName] == null) 
					{
						local_provider = new SampleSettingsProvider();
						local_provider.Initialize(null, null);
						Providers.Add (local_provider);
					}
	
					if (local_provider != null)
					{
						foreach (SettingsProperty prop in properties)
						{
							prop.Provider = local_provider;
						}
					}
					return properties;
				}
			}
		}

    }
}
