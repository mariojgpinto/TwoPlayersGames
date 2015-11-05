using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class MySerializer {
	static SurrogateSelector ss = null;

	static Dictionary<string, object> dict = new Dictionary<string, object>();

	#region SURRUGATES
	sealed class Vector2SerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Vector2 object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Vector2 v2 = (Vector2) obj;
			info.AddValue("x", v2.x);
			info.AddValue("y", v2.y);
		}
		
		// Method called to deserialize a Vector2 object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {
			
			Vector2 v2 = (Vector2) obj;
			v2.x = (float)info.GetValue("x", typeof(float));
			v2.y = (float)info.GetValue("y", typeof(float));
			obj = v2;
			return obj;
		}
	}

	sealed class Vector3SerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Vector3 object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Vector3 v3 = (Vector3) obj;
			info.AddValue("x", v3.x);
			info.AddValue("y", v3.y);
			info.AddValue("z", v3.z);
		}
		
		// Method called to deserialize a Vector3 object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {
			
			Vector3 v3 = (Vector3) obj;
			v3.x = (float)info.GetValue("x", typeof(float));
			v3.y = (float)info.GetValue("y", typeof(float));
			v3.z = (float)info.GetValue("z", typeof(float));
			obj = v3;
			return obj;
		}
	}
	
	sealed class Vector4SerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Vector4 object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Vector4 v4 = (Vector4) obj;
			info.AddValue("x", v4.x);
			info.AddValue("y", v4.y);
			info.AddValue("z", v4.z);
			info.AddValue("w", v4.w);
		}
		
		// Method called to deserialize a Vector4 object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {
			
			Vector4 v4 = (Vector4) obj;
			v4.x = (float)info.GetValue("x", typeof(float));
			v4.y = (float)info.GetValue("y", typeof(float));
			v4.z = (float)info.GetValue("z", typeof(float));
			v4.w = (float)info.GetValue("w", typeof(float));
			obj = v4;
			return obj;
		}
	}

	sealed class QuaternionSerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Quaternion object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Quaternion q = (Quaternion) obj;
			info.AddValue("x", q.x);
			info.AddValue("y", q.y);
			info.AddValue("z", q.z);
			info.AddValue("w", q.w);
		}
		
		// Method called to deserialize a Quaternion object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {
			
			Quaternion q = (Quaternion) obj;
			q.x = (float)info.GetValue("x", typeof(float));
			q.y = (float)info.GetValue("y", typeof(float));
			q.z = (float)info.GetValue("z", typeof(float));
			q.w = (float)info.GetValue("w", typeof(float));
			obj = q;
			return obj;
		}
	}

	sealed class ColorSerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Color object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Color clr = (Color) obj;
			info.AddValue("r", clr.r);
			info.AddValue("g", clr.g);
			info.AddValue("b", clr.b);
			info.AddValue("a", clr.a);
		}
		
		// Method called to deserialize a Color object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {
			
			Color clr = (Color) obj;
			clr.r = (float)info.GetValue("r", typeof(float));
			clr.g = (float)info.GetValue("g", typeof(float));
			clr.b = (float)info.GetValue("b", typeof(float));
			clr.a = (float)info.GetValue("a", typeof(float));
			obj = clr;
			return obj;
		}
	}

	sealed class Texture2DSerializationSurrogate : ISerializationSurrogate {		
		// Method called to serialize a Color object
		public void GetObjectData(System.Object obj,
		                          SerializationInfo info, StreamingContext context) {
			
			Texture2D texture = (Texture2D) obj;
			
			Debug.Log("GetObjectData:\n" +
			          "Width:" +texture.width + "\n" + 
			          "Height:" +texture.height + "\n" + 
			          "Format:" +texture.format + "\n" + 
			          "");
			info.AddValue("width", texture.width);
			info.AddValue("height", texture.height);
			info.AddValue("format", texture.format);
			//info.AddValue("values", texture.EncodeToPNG());
			info.AddValue("values", texture.GetRawTextureData());
		}
		
		// Method called to deserialize a Color object
		public System.Object SetObjectData(System.Object obj,
		                                   SerializationInfo info, StreamingContext context,
		                                   ISurrogateSelector selector) {

			int w = (int)info.GetValue("width", typeof(int));
			int h = (int)info.GetValue("height", typeof(int));
			TextureFormat format = (TextureFormat)info.GetValue("format", typeof(int));

			Texture2D texture = new Texture2D(w,h,format,false);

			Debug.Log("SetObjectData:\n" +
			          "Width:" + w + "\n" + 
			          "Height:" + h + "\n" + 
			          "Format:" + format + "\n" + 
			          "");

			//texture.LoadImage((byte[])info.GetValue("values", typeof(byte[])));
			texture.LoadRawTextureData((byte[])info.GetValue("values", typeof(byte[])));
			texture.Apply();
			obj = texture;
			return obj;
		}
	}
	#endregion

	static void CreateSurrogates(){
		ss = new SurrogateSelector();
		
		//VECTOR2
		Vector2SerializationSurrogate v2_ss = new Vector2SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), v2_ss);
		//VECTOR3
		Vector3SerializationSurrogate v3_ss = new Vector3SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3_ss);
		//VECTOR4
		Vector4SerializationSurrogate v4_ss = new Vector4SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), v4_ss);
		
		//QUATERNION
		QuaternionSerializationSurrogate q_ss = new QuaternionSerializationSurrogate();
		ss.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), q_ss);
		
		//COLOR AND COLOR32
		ColorSerializationSurrogate color_ss = new ColorSerializationSurrogate();
		ss.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), color_ss);
		ss.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), color_ss);

		Texture2DSerializationSurrogate texture_ss = new Texture2DSerializationSurrogate();
		ss.AddSurrogate(typeof(Texture2D), new StreamingContext(StreamingContextStates.All), texture_ss);
	}

	public static void Save(object obj, string path){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (path);

		if(ss == null){
			CreateSurrogates();
		}

		bf.SurrogateSelector = ss;
		
		bf.Serialize(file, obj );
		file.Close();
	}

	public static object Load(string path){
		if(File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter();
			
			if(ss == null){
				CreateSurrogates();
			}
			
			bf.SurrogateSelector = ss;
			
			FileStream file = File.Open(path, FileMode.Open);
			object obj = bf.Deserialize(file);
			file.Close();		

			return obj;
		}

		return null;
	}

	public static bool SaveDictionary(string path){
		if(dict.Count > 0){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (path);
			
			if(ss == null){
				CreateSurrogates();
			}
			
			bf.SurrogateSelector = ss;
			
			bf.Serialize(file, dict);
			file.Close();

			return true;
		}

		return false;
	}
	
	public static bool LoadDictionary(string path){
		if(File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter();
			
			if(ss == null){
				CreateSurrogates();
			}
			
			bf.SurrogateSelector = ss;
			
			FileStream file = File.Open(path, FileMode.Open);
			dict = (Dictionary<string,object>)bf.Deserialize(file);
			file.Close();	

			return true;
		}
		
		return false;
	}

	public static bool AddToDictionary(string key, object obj){
		if(!dict.ContainsKey(key)){
			dict.Add(key,obj);
			return true;
		}
		return false;
	}

	public static object GetFromDictionary(string key){
		if(dict.ContainsKey(key))
			return dict[key];
		return null;
	}
}