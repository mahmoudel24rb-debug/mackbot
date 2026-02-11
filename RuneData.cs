// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Dysonity.Logic.RuneData
using System.Runtime.CompilerServices;

internal class RuneData
{
	[CompilerGenerated]
	private long _003CRuneGid_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CSC_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CSN_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CEC_003Ek__BackingField;

	[CompilerGenerated]
	private int _003CTotalNumber_003Ek__BackingField;

	internal long RuneGid
	{
		[CompilerGenerated]
		get
		{
			return _003CRuneGid_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			byte b = 183;
			_003CRuneGid_003Ek__BackingField = value;
		}
	}

	internal int SC
	{
		[CompilerGenerated]
		get
		{
			return _003CSC_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			int num = 170;
			do
			{
				_003CSC_003Ek__BackingField = value;
			}
			while (839868564 >> (941075211 >> num) == 0);
		}
	}

	internal int SN
	{
		[CompilerGenerated]
		get
		{
			byte b = 0;
			return _003CSN_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CSN_003Ek__BackingField = value;
		}
	}

	internal int EC
	{
		[CompilerGenerated]
		get
		{
			byte b = 1;
			return _003CEC_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CEC_003Ek__BackingField = value;
		}
	}

	internal int TotalNumber
	{
		[CompilerGenerated]
		get
		{
			ushort num = 43099;
			return _003CTotalNumber_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			_003CTotalNumber_003Ek__BackingField = value;
		}
	}

	internal long TotalPrice { get; set; }

	public RuneData()
	{
		short num = 14914;
		_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(404224392 + num >>> 31) % (uint)(~num)) / ((-710392289 * num >>> 19) * (num % 1907829292)) - -2099](this);
	}
}
