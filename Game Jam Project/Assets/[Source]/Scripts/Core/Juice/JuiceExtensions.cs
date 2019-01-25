using UnityEngine;

public static class JuiceExtensions
{
	public static void Shake (this Camera cam)
	{
		Screenshake shake = cam.transform.GetComponent<Screenshake>();
		if(shake != null)
		{
			shake.Shake();
		}
		else
		{
			Debug.Log("Camera doesn't have Screenshake component, adding one!");
			cam.transform.gameObject.AddComponent<Screenshake>().Shake(); //add and shake
		}
	}
	public static void Shake (this Camera cam, float multi)
	{
		Screenshake shake = cam.transform.GetComponent<Screenshake>();
		if(shake != null)
		{
			shake.Shake(multi);
		}
		else
		{
			Debug.Log("Camera doesn't have Screenshake component, adding one!");
			cam.transform.gameObject.AddComponent<Screenshake>().Shake(multi); //add and shake
		}
	}
	
	public static void Kick (this Camera cam, Vector3 dir)
	{
		Kickback kick = cam.transform.GetComponent<Kickback>();
		if(kick != null)
		{
			kick.Kick(dir);
		}
		else
		{
			Debug.Log("Camera doesn't have Kickback component, adding one!");
			cam.transform.gameObject.AddComponent<Kickback>().Kick(dir); //add and shake
		}
	}
	public static void Kick (this Camera cam, Vector3 dir, float multi)
	{
		Kickback kick = cam.transform.GetComponent<Kickback>();
		if(kick != null)
		{
			kick.Kick(dir, multi);
		}
		else
		{
			Debug.Log("Camera doesn't have Kickback component, adding one!");
			cam.transform.gameObject.AddComponent<Kickback>().Kick(dir, multi); //add and shake
		}
	}
}