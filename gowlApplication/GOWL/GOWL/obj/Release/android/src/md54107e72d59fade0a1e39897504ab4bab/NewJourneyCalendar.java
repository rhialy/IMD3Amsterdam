package md54107e72d59fade0a1e39897504ab4bab;


public class NewJourneyCalendar
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("GOWL.NewJourneyCalendar, GOWL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NewJourneyCalendar.class, __md_methods);
	}


	public NewJourneyCalendar () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NewJourneyCalendar.class)
			mono.android.TypeManager.Activate ("GOWL.NewJourneyCalendar, GOWL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
