using NAudio.Wave;

namespace EntryManagement.Service
{
    public  class SoundService
{

        public void PlaySoundCamera()
        {
          
                try
                {
                    using (var audioFile = new AudioFileReader("C:\\Users\\khoav\\OneDrive\\Tài liệu\\ADO.NET\\SESSION\\Camera\\Tiếng⧸âm thanh đồng xu (tiền) để ghép vào video ｜ Coin (money) sound effect for edit vi.wav"))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                        int IsCount = 0;
                    Console.WriteLine("Press Enter to stop playback...");

                    while (outputDevice.PlaybackState == PlaybackState.Playing && IsCount <= 5)
                    {
                        Thread.Sleep(1000);
                        IsCount++;
                        var key = Console.ReadLine();
                        if (key != "null")
                        {
                            break;
                        }
                    }

                    outputDevice.Stop();
                    }

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error Sound : " + ex.Message);
                }
            }
    public void PlaySoundLog()
    {

        try
        {
            using (var audioFile = new AudioFileReader("C:\\Users\\khoav\\OneDrive\\Tài liệu\\ADO.NET\\SESSION\\Mail\\Alter.wav"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                int IsCount = 0;

                    Console.WriteLine("Press Enter to stop playback...");

                    while (outputDevice.PlaybackState == PlaybackState.Playing && IsCount <= 10)
                    {
                        Thread.Sleep(1000);
                        IsCount++;

                        var key = Console.ReadLine();
                        if (key != "null")
                        {
                            break;
                        }
                    }


                    outputDevice.Stop();
            }

        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Error Sound : " + ex.Message);
        }
    }
}
}