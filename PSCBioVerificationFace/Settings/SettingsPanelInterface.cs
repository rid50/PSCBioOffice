
namespace Neurotec.Samples
{
	public interface SettingsPanelInterface
	{
		string DisplayName { get; }
		void SaveSettings();  //save display settings to file and apply them to coresponding object
		void LoadSettings(); //load settings for display purposes
		void LoadDefaultSettings(); //load default settings for display purposes
	}
}
