using GrabzIt.COM;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class AnimationOptions : BaseOptions, IAnimationOptions
    {
        public AnimationOptions()
        {
	        CustomWaterMarkId = string.Empty;
	        Width = 0;
	        Height = 0;	
	        Start = 0; 
	        Duration = 1;
	        Speed = 0;
	        FramesPerSecond = 0;
	        Repeat = 0;
	        Reverse = false;
        }
        /// <summary>
        /// The width of the resulting animated GIF in pixels
        /// </summary>
        public int Width
        {
            get;
            set;
        }
        
        /// <summary>
        /// The height of the resulting animated GIF in pixels
        /// </summary>
        public int Height
        {
            get;
            set;
        }
        
        /// <summary>
        /// The starting position of the video that should be converted into a animated GIF
        /// </summary>
        public int Start
        {
            get;
            set;
        }
        
        /// <summary>
        /// The length in seconds of the video that should be converted into a animated GIF
        /// </summary>
        public int Duration
        {
            get;
            set;
        }
        
        /// <summary>
        /// The speed of the animated GIF from 0.2 to 10 times the original speed
        /// </summary>
        public float Speed
        {
            get;
            set;
        }
        
        /// <summary>
        /// The number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60
        /// </summary>
        public float FramesPerSecond
        {
            get;
            set;
        }
        
        /// <summary>
        /// The number of times to loop the animated GIF. If 0 it will loop forever
        /// </summary>
        public int Repeat
        {
            get;
            set;
        } 
        
        /// <summary>
        /// If true the frames of the animated GIF are reversed
        /// </summary>
        public bool Reverse
        {
            get;
            set;
        }
        
        /// <summary>
        /// Add a custom watermark to the animated GIF
        /// </summary>
        public string CustomWaterMarkId
        {
            get;
            set;
        }

        /// <summary>
        /// The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
        /// </summary>
        public int Quality
        {
            get;
            set;
        }

        internal override string GetSignatureString(string applicationSecret, string callBackURL, string url)
        {
            string urlParam = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                urlParam = url + "|";
            }

            string callBackURLParam = string.Empty;
            if (!string.IsNullOrEmpty(callBackURL))
            {
                callBackURLParam = callBackURL;
            }

            return applicationSecret + "|" + urlParam + callBackURLParam +
            "|" + Height + "|" + Width + "|" + CustomId + "|" + FramesPerSecond + "|" + Speed + "|" + Duration + "|" + Repeat + "|" + Convert.ToInt32(Reverse) +
            "|" + Start + "|" + CustomWaterMarkId + "|" + ConvertCountryToString(Country) + "|" + Quality + "|" + ExportURL + "|" + EncryptionKey;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("width", Width.ToString());
            parameters.Add("height", Height.ToString());
            parameters.Add("duration", Duration.ToString());
            parameters.Add("speed", Speed.ToString());
            parameters.Add("customwatermarkid", CustomWaterMarkId);
            parameters.Add("start", Start.ToString());
            parameters.Add("fps", FramesPerSecond.ToString());
            parameters.Add("repeat", Repeat.ToString());
            parameters.Add("reverse", Convert.ToInt32(Reverse).ToString());
            parameters.Add("quality", Quality.ToString());

            return parameters;
        }
    }
}
