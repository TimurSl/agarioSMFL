namespace ZenisoftGameEngine.Sound;

public class AudioPlayer
{
	public List<string> AudioClipsList = new List<string>();
	
	private static Dictionary<string, SFML.Audio.Sound> audioClips = new Dictionary<string, SFML.Audio.Sound>();

	public void LoadAudioClips()
	{
		audioClips.Clear();
		foreach(string pathToClip in AudioClipsList)
		{
			string name = pathToClip.Split('\\').Last().Split('.').First();
			
			audioClips.Add(name, new SFML.Audio.Sound(new SFML.Audio.SoundBuffer(pathToClip)));
		}
	}
	
	public static void PlayAudioClip(string name, bool loop = false)
	{
		if (audioClips.TryGetValue(name, out var audioClip))
		{
			audioClip.Loop = loop;
			audioClip.Play();
		}
	}
	
	public void SetVolume(float volume) 
	{
		foreach (var audioClip in audioClips)
		{
			audioClip.Value.Volume = volume;
		}
	}
}