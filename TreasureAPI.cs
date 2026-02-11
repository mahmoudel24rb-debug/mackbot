// Jitsuri, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DofusLibrary.Common.Treasure.TreasureAPI
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DofusLibrary.Common.Treasure;
using Newtonsoft.Json;

public static class TreasureAPI
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CGetNearbyPoiFromApi_003Ed__0 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<NearbyPoi> _003C_003Et__builder;

		public string direction;

		public int poiId;

		public int x;

		public int y;

		private HttpClient _003Cclient_003E5__2;

		private StringContent _003Ccontent_003E5__3;

		private int _003CmaxRetries_003E5__4;

		private int _003CdelayBetweenRetriesMs_003E5__5;

		private int _003Cattempt_003E5__6;

		private TaskAwaiter<HttpResponseMessage> _003C_003Eu__1;

		private TaskAwaiter<string> _003C_003Eu__2;

		private TaskAwaiter _003C_003Eu__3;

		private void MoveNext()
		{
			int num = _003C_003E1__state;
			NearbyPoi result = default(NearbyPoi);
			byte b = default(byte);
			try
			{
				int num2 = default(int);
				if ((uint)num > 4u)
				{
					num2 = 2;
					_003Cclient_003E5__2 = _671BC22C.BF9F3D1F.FE8E0C9E[-2147454297 + (int.MinValue ^ ((ushort)num2 | -28753))]();
				}
				try
				{
					if ((uint)num <= 3u)
					{
						goto IL_0308;
					}
					if (num == 4)
					{
						goto IL_1ad2;
					}
					string _97B83B = MapDirection(direction);
					short num3 = default(short);
					sbyte b2 = default(sbyte);
					while (true)
					{
						IL_0058:
						string _2F26EBA = _671BC22C.BF9F3D1F.FE8E0C9E[1563](new global::_992A2E04<int, string, string, string>(poiId, _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref x), _671BC22C.BF9F3D1F.FE8E0C9E[1851](ref y), _97B83B));
						b = 116;
						b = (byte)(-116 ^ (-b >> 0));
						while (true)
						{
							switch ((uint)b % 5u)
							{
							default:
								b = (byte)((((b + -742519884) / ~b) | b) + -742519768);
								_003Ccontent_003E5__3 = _671BC22C.BF9F3D1F.FE8E0C9E[1592 + ((int)((((uint)(-1969416960 % b) > (uint)b / 127128360u) ? 1u : 0u) ^ (uint)b) >> -2129559507 % b % (1308762007 * -b))](_2F26EBA, _671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)(~(-358767455 / b)) | ((uint)b % 2739101260u)) + -3092617](), "application/json");
								num3 = (short)(((2886729529u < (uint)(short)b) ? 1 : 0) + ~(b >>> ~b));
								continue;
							case 1u:
								_003CmaxRetries_003E5__4 = 3 + ((-(b * num3) == 1722347648) ? 1 : 0);
								num3 = (((uint)((short)b | -576722555) / (uint)(~(346081579 >>> (int)num3)) == (uint)((int)(11926053 % ((uint)num3 % (uint)b)) * (int)num3)) ? ((short)1) : ((short)0));
								b = (byte)(152 + ~b % ~(-1299755084 * (int)((uint)num3 / (uint)(~num3))));
								continue;
							case 2u:
								goto IL_01d0;
							case 3u:
								b = (byte)((num3 >>> 16 << b - -2076736880) ^ 0x74);
								break;
							case 4u:
								b = (byte)((uint)(0x3804D3A5 & num3) % ((uint)(1286473377 % (-1416723966 ^ b)) % (uint)(b | 0x69695455)) + 116);
								break;
							}
							_003Cattempt_003E5__6 = ((623196453 > (0x363258BF ^ num3)) ? 1 : 0) - -1;
							if ((uint)(1049701429 << (int)(3229636389u % (uint)b) / ~num3) >= (uint)((b >> 8) - ~(b ^ b)))
							{
								break;
							}
							b2 = (sbyte)((uint)((-1776439901 % b) & b) / 3653903298u);
							b = (byte)((((((num3 > num3 << (int)num3) ? 1u : 0u) < (uint)num3) ? 1u : 0u) & 0u) - 4294967292u);
							continue;
							IL_01d0:
							b = (byte)(~(~num3 % ~(num3 << (int)b)) - -117);
							_003CdelayBetweenRetriesMs_003E5__5 = (int)(0x1388 ^ ((uint)((b >>> (int)b) % -1836979167) / (uint)(~((int)(4003728915u / (uint)(0x7C3951B5 ^ b)) >> (int)b))));
							if ((uint)(-13053526 ^ num3) >= (uint)(~(b & num3) >> 5))
							{
								goto IL_0058;
							}
							b = (byte)(-11259278 ^ ((int)((uint)(-(-1306077804 >> (int)num3)) / (uint)b) / ~(0 - ((-426774863 - num3 > num3) ? 1 : 0))));
						}
						break;
					}
					goto IL_1c65;
					IL_1c65:
					b2 = (sbyte)((num3 & -1129646690) / ~num3 * 1000888736 * 268468741);
					b = (byte)((b2 % (1226996755 << (b2 ^ b2))) ^ (num3 % 3606976 / -1749524597) ^ 0x8D);
					goto IL_186b;
					IL_186b:
					TaskAwaiter awaiter = default(TaskAwaiter);
					uint num4 = default(uint);
					int num5 = default(int);
					while (true)
					{
						switch ((uint)b % 12u)
						{
						default:
							b = (byte)((b ^ 0x26690DDE) - 644418943);
							awaiter = _671BC22C.BF9F3D1F.FE8E0C9E[0x286 ^ ((419552033 >>> (int)b) * b)](_671BC22C.BF9F3D1F.FE8E0C9E[1476 + ~(((b / -544765438) | 0x7F92791C) % b)](_003CdelayBetweenRetriesMs_003E5__5));
							b = (byte)((1361519291 / b) & b & b);
							b = (byte)(b ^ 0x542C3327 ^ 0x542C3348);
							continue;
						case 1u:
							break;
						case 2u:
							goto IL_199e;
						case 3u:
							b = (byte)((((num4 & b) * 832088997 + 598354328) | (uint)((int)num4 % 1413322297)) - 598354254);
							goto IL_1a2b;
						case 4u:
							goto IL_1a58;
						case 5u:
							b = (byte)(0x4F ^ -(-(b % (num2 % -1699947603))));
							return;
						case 6u:
							b = (byte)(((uint)(-1607608682 % ~(short)(num4 ^ num4)) & (3761786552u / (1930736298 / num4))) + 74);
							_671BC22C.BF9F3D1F.FE8E0C9E[(int)(num4 - 1922233) + -881211553](ref awaiter);
							b = (byte)(1508703487 - b);
							b = (byte)((num4 << (int)num4) % (uint)(~(1 >>> (int)(num4 & (uint)((int)num4 % -527308501)))) + 2990014471u);
							continue;
						case 7u:
							goto IL_1bce;
						case 8u:
							goto IL_1c2e;
						case 9u:
							goto IL_1caf;
						case 10u:
							b = (byte)((int)((uint)b / (uint)(~(b % b)) >> (int)b) + -(-385218511 + b) - 385218200);
							result = null;
							if (((uint)(-682098817 / -b) ^ ((uint)(b ^ -1084372670) / (uint)(b << ~b))) != 0)
							{
								b = (byte)((int)((uint)b % 2404726597u) % (b << ((681942054 > b) ? 1 : 0)) - 98);
								continue;
							}
							goto IL_1a2b;
						case 11u:
							{
								b = (byte)(253u + ((1877092471u < (uint)b) ? 1u : 0u));
								goto end_IL_003b;
							}
							IL_1a2b:
							_003C_003Eu__3 = awaiter;
							num2 = b;
							b = (byte)(((b < -1896276274) ? 1 : 0) - ((b < num4 % 437006612) ? 1 : 0) - -52);
							continue;
						}
						break;
						IL_1c2e:
						b = (byte)(-76047422 + (int)num4 % ~((int)num4 * -1088878332));
						_003Cattempt_003E5__6 = num5 + (-179 + ~(-b));
						num3 = 0;
						b ^= 0xC1;
						goto IL_1c65;
						IL_1bce:
						b = (byte)(0x34A393C7 ^ num4);
						num5 = _003Cattempt_003E5__6;
						if ((0x6DF872F1 | num4) % 255043 != 0)
						{
							b = (byte)((-37769963 % (1511257387 >> (int)(b | num4)) >>> (int)(2945505849u / (num4 | 0xBDB3900Cu) >> (int)num4)) + 199);
							continue;
						}
						goto IL_1812;
						IL_199e:
						num = (_003C_003E1__state = ((int)(654426412 - (uint)(b / b) / 2175799425u) >> 3) - 81803297);
						if (((uint)(0 - ((b + 185479179 == (int)(0 - num4)) ? 1 : 0)) | ((uint)(1653724215 - ((int)num4 - -8257618)) & (((int)(num4 & 0xD4AF4E9Du) > 1394022546 << (int)num4) ? 1u : 0u))) == 0)
						{
							b = (byte)((b | 0) + -23);
							continue;
						}
						goto IL_1c65;
						IL_1a58:
						b = (byte)((0 - ((2082810924u > (uint)(num2 >> 31)) ? 1 : 0)) ^ -75);
						_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						if (-((ushort)(-1331399543 >> num2) - ((int)(sbyte)num2 - (int)num4)) != 0)
						{
							b = (byte)(554973448 + (b & -862690640) % (-467237104 >> (int)b) + -554973443);
							continue;
						}
						goto IL_1cc8;
					}
					b = (byte)((b >> 6) - -74);
					goto IL_1934;
					IL_1af4:
					if (((-(b % b) * (((-711870716 ^ b) == 675598959) ? 1 : 0)) | -1028729165) <= (int)(0 - (4021867673u % (uint)(b | 0x5CB4238A) >> b * -39113974)))
					{
						num4 = (uint)(0x34A39338 ^ b);
						if ((short)(0u - ((num4 > 2973235372u) ? 1u : 0u)) != 0)
						{
							return;
						}
						b = (byte)(54 + (uint)(b % b) % 7u);
						goto IL_186b;
					}
					goto IL_1ae6;
					IL_0308:
					DefaultInterpolatedStringHandler D6A007B = default(DefaultInterpolatedStringHandler);
					try
					{
						TaskAwaiter<HttpResponseMessage> awaiter3 = default(TaskAwaiter<HttpResponseMessage>);
						TaskAwaiter<string> awaiter2 = default(TaskAwaiter<string>);
						string item = default(string);
						HttpResponseMessage result3 = default(HttpResponseMessage);
						string result2 = default(string);
						int num6 = default(int);
						string result4 = default(string);
						while (true)
						{
							IL_0308_2:
							switch (num)
							{
							default:
								awaiter3 = _671BC22C.BF9F3D1F.FE8E0C9E[1595](_003Cclient_003E5__2, _8E94B429._8611DC0E, _003Ccontent_003E5__3).GetAwaiter();
								num4 = 3741315528u;
								num4 = (uint)((-541846748 >>> (int)(num4 - (ushort)(num4 % 3657524911u))) - -1496408472);
								goto IL_036b;
							case 0:
								num3 = 2862;
								if (num3 == 0)
								{
									goto IL_0548;
								}
								awaiter3 = _003C_003Eu__1;
								b = (((int)(sbyte)(((num3 == num3) ? 1u : 0u) / (uint)(~(num3 >>> 14))) < ((num3 == 922934452) ? 1 : 0)) ? ((byte)1) : ((byte)0));
								if (((858507935 * (b >>> 28)) ^ -2044091716) == 0)
								{
									continue;
								}
								goto IL_05a4;
							case 1:
								awaiter2 = _003C_003Eu__2;
								num3 = -25573;
								if (num3 % num3 % (int)(0xE4374D08u ^ (3179977388u / (uint)num3)) == 0)
								{
									_003C_003Eu__2 = default(TaskAwaiter<string>);
									if (~((456805176u > (uint)num3) ? 1u : 0u) == 0)
									{
										continue;
									}
									num = (_003C_003E1__state = ~num3 + -25573);
									num4 = 1285835871u;
									num2 = 6;
								}
								goto IL_08d2;
							case 2:
								b = 104;
								awaiter2 = _003C_003Eu__2;
								b = (byte)(b | 0x17E2E54F);
								_003C_003Eu__2 = default(TaskAwaiter<string>);
								b2 = (sbyte)(((1921209985 << (int)b) ^ -2119320007) << 29);
								goto IL_0fa1;
							case 3:
								{
									num4 = 50u;
									if (~num4 % 21 != 0)
									{
										num4 = (uint)((short)(num4 % 3097387449u) ^ -341306999);
										goto IL_036b;
									}
									goto IL_0c76;
								}
								IL_0548:
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter3, ref this);
								return;
								IL_082c:
								do
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
								}
								while (num2 == 0);
								num4 = (uint)(261430419 + num2 * 1174516910);
								goto IL_036b;
								IL_036b:
								while (true)
								{
									switch (num4 % 32)
									{
									case 1u:
										num4 = (uint)(-1687432574 + (0x43941FB1 ^ b));
										_003C_003Eu__1 = awaiter3;
										num2 = (int)(ushort)(~(2065901736 - num4)) / (int)((uint)((int)(num4 / num4) * -1878006760) / (uint)b);
										if ((uint)(-216957418 * ((b & b) ^ -1489430873)) + (0x299C2E15 ^ (num4 - 755519005)) != 0)
										{
											num4 = (uint)(num2 ^ 0x6A ^ -1801919384);
											continue;
										}
										return;
									case 2u:
										goto IL_050d;
									case 3u:
										goto IL_0631;
									case 4u:
										goto IL_06dd;
									case 5u:
										goto IL_077e;
									case 6u:
										goto end_IL_036b;
									case 7u:
										num4 = (uint)((0x418141A7 & (-1246916443 >>> (int)(0 - num4))) + -2);
										return;
									case 8u:
										num4 = (uint)((((1413460908 << (int)num4) & -14208) >>> (int)((uint)(num2 - b) / 3525633437u - 1462187439)) - -1285831795);
										item = CE0A9D0F.AFB39003(awaiter2.GetResult()).data;
										if ((int)((uint)(((b > 515206534) ? 1 : 0) - num2 * -1120517063) % (uint)(-(b ^ -298087651))) < -684087376 + (int)num4)
										{
											num4 = (uint)num2 / 1795558828u - 1448565591;
											continue;
										}
										goto end_IL_0308;
									case 9u:
										goto IL_0986;
									case 10u:
										num4 = (uint)(0x4CA44C5D ^ (num2 & 0x6E7A92CA));
										goto end_IL_003b;
									case 11u:
										goto IL_0a72;
									case 12u:
										goto IL_0b4f;
									case 13u:
										goto IL_0bce;
									case 14u:
										goto IL_0c66;
									case 15u:
										goto IL_0cd5;
									case 16u:
										goto IL_0d7c;
									case 17u:
										num4 = 1743169516 + 2551797781u / (uint)((int)b2 % (int)(0x72122BE0 & num4));
										awaiter2 = _671BC22C.BF9F3D1F.FE8E0C9E[-270713032 ^ (((b2 << 27 > -2061278055) ? 1 : 0) + -270713919)](_671BC22C.BF9F3D1F.FE8E0C9E[1285544330 / (562150793 / b) / (int)(num4 * (num4 & 0xE2AB98BFu)) - -1903](result3)).GetAwaiter();
										num3 = (short)(b2 >>> -b);
										if ((uint)(564308501 << (int)((uint)b2 % 284927351u) >>> (int)b2) < (uint)((int)num4 % (b2 + (b >> num3 % b2))))
										{
											return;
										}
										num4 = (uint)(0x26B4E012 ^ (short)(((b2 << 19) & num3) ^ (b2 - b)));
										continue;
									case 18u:
										goto IL_0eb6;
									case 19u:
										num4 = (uint)(-28399631 ^ (-28399632 | b2));
										_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
										return;
									case 20u:
										goto IL_0fe9;
									case 21u:
										num4 = b;
										_671BC22C.BF9F3D1F.FE8E0C9E[(int)((uint)b2 / (uint)b2 << (int)(((-77 > b) ? 1u : 0u) >> 26)) - -1675](_671BC22C.BF9F3D1F.FE8E0C9E[(int)((((uint)num3 < (uint)(b2 % 1812698280)) ? 1u : 0u) << (int)b) - -1795]("Non-retryable error: ", result2));
										goto end_IL_0308;
									case 22u:
										goto IL_10c3;
									case 23u:
										goto IL_119b;
									case 24u:
										goto IL_1211;
									case 25u:
										goto IL_128f;
									case 26u:
										goto IL_131e;
									case 27u:
										goto IL_1374;
									case 28u:
										goto IL_13ea;
									case 29u:
										goto IL_1452;
									case 30u:
										goto IL_14ef;
									case 31u:
										num4 = (uint)((764632241 << (int)num3) - 764632241);
										goto end_IL_0308;
									}
									num4 = (num4 | ((((int)num4 < (int)num4) ? 1u : 0u) >> 30)) % (uint)(~(-284244354 / (int)num4)) + 2244792520u;
									if (!awaiter3.IsCompleted)
									{
										num = (_003C_003E1__state = 0x1137CB8 ^ ((-2143379022 >>> (1026584594 >>> (int)num4)) * (int)(~(0xA19391B | num4))));
										b = (byte)((0xD8796E69u & (num4 & 0x3E2B8C8F)) ^ (~num4 | (num4 >> (int)(num4 + num4))));
										if ((uint)b <= (uint)((int)(num4 + (0xCA2DA78Eu | num4)) % ((int)num4 % -801514489)))
										{
											num4 = (uint)(-1978101503 ^ ((b << 5) % ~(b % -(-b))));
											continue;
										}
										goto end_IL_0308;
									}
									goto IL_0612;
									IL_14ef:
									num4 = 0xBC6EA6C7u ^ (0 - (num4 + 27));
									goto IL_14fd;
									IL_1452:
									num4 = (uint)((ushort)(~(((uint)num2 > (uint)(244318098 - num2)) ? 1 : 0)) + -65484);
									num = (_003C_003E1__state = -1 + (int)(((uint)num2 % (uint)num2) & num4));
									b2 = 1;
									num4 ^= 0x32;
									b = 1;
									num3 = 0;
									goto IL_148c;
									IL_13ea:
									num4 = (uint)(0x32 ^ ((-1466225665 * (1169594638 >>> (int)num4)) & ((-1733304407 - num2) / -1322141407)));
									_003C_003Eu__2 = default(TaskAwaiter<string>);
									if (num2 + 1436376321 != 0)
									{
										num4 = 1759490590 + (1415389312 - ((uint)num2 % 3274255541u - num4) >> (num2 >> 18 >>> 25));
										continue;
									}
									goto case 3;
									IL_0bce:
									num4 = (uint)(-203406761 ^ (-1507821138 % ((-652207190 ^ ~b) - -(b >>> 29))));
									_671BC22C.BF9F3D1F.FE8E0C9E[-270343 + b - -272018](_671BC22C.BF9F3D1F.FE8E0C9E[((num4 < 4131527385u) ? 1u : 0u) ^ 0x3A7u](ref D6A007B));
									if ((-197366379 ^ b) <= (int)(1387072557 / num4) + -921052885)
									{
										num4 = (uint)((int)(~(num4 + b)) % (int)((0x76B5CC10 | (num4 * 1813200427)) & (num4 & b)) - -119758734);
										continue;
									}
									goto IL_07a3;
									IL_1374:
									num4 = (uint)((((int)num4 >> (int)num4) + ((int)num4 >> (int)num4)) ^ -56);
									awaiter2 = _003C_003Eu__2;
									num2 = 0x9AFD933 ^ ((((byte)num4 ^ (num4 % 3928386565u)) > ~(3859891000u % num4)) ? 1 : 0);
									if (num4 != 0)
									{
										num4 = (uint)(~((int)num4 / -1766687472 << ((num4 == 2922104488u) ? 1 : 0)) * ((num2 - -73789644) / -182699976) - -949702171);
										continue;
									}
									goto case 0;
									IL_1086:
									num4 = (uint)((b2 - b) ^ (b / (1553390217 << (int)(sbyte)b)));
									num4 = ((uint)(-(~b2) >> (int)num4) / (uint)(~(-1358653798 % b * (int)(~num4)))) ^ 0x17BC6F36;
									continue;
									IL_128f:
									num4 = ((3700665601u > (uint)(b - -1324500555)) ? 1u : 0u) - 1u;
									_003C_003Eu__2 = awaiter2;
									b = (byte)((uint)((b2 >> (int)b2) % -b) >> 3);
									if (1554405532 >> -1323917765 - (int)(279172399 + num4) >= (((uint)num3 < (uint)b2) ? 1 : 0) / 1335359884 << (int)((uint)(byte)b2 / (uint)b2))
									{
										num4 = (uint)((132 % ~((num3 >>> (int)b) / b2 << ((int)(num4 ^ 0x28BB7607) >> (int)num4))) ^ 0x123CE53A);
										continue;
									}
									goto IL_0612;
									IL_0b4f:
									num4 = ((((num4 << (int)num4) - 141) & num4) | num4) ^ 0x1B83E32D;
									D6A007B.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[1586 + b * (((b > num4) ? 1u : 0u) & 0x61B46DCDu)](result3));
									if ((2827772427u % num4 + 974647347) * ((1050242948 % num4) & 0xAE) >> 4 == 0)
									{
										num4 = (uint)(0x15142A19 ^ (2074744084 / b));
										continue;
									}
									goto IL_05a4;
									IL_1211:
									num4 = (uint)((1778916284 << (int)(~(num4 * b)) >>> (int)num3) - 232652800);
									num = (_003C_003E1__state = -32 + (int)((uint)(-148679631 / (int)(~((b2 == num3) ? 1u : 0u))) % (uint)(-b) >> (((0x5F93AC8E | num3) * ~num3) | (b - -1391507702))));
									if (-350046927 + b2 != 0)
									{
										num4 = (uint)(0x48CCF9B2 ^ -((num3 * b) | ~(1326686635 * b)));
										continue;
									}
									goto IL_0acf;
									IL_050d:
									num4 = (uint)(-553651768 + ((1561130555 * (-853980153 >> (int)num4)) | ((-41130229 & b) + 1686186298)) % (int)(~(0u / (uint)b / (uint)(1322891255 << (int)(short)num2))));
									goto IL_0548;
									IL_119b:
									num4 = 2935116538u % (uint)(b2 / ~(0 - ((1219740167u < (uint)num3) ? 1 : 0))) - 2935116538u;
									if (!awaiter2.IsCompleted)
									{
										b = (byte)((0 - 2827874447u / (uint)(2015291830 >>> (int)b)) ^ (uint)num3);
										if (num3 - ((num3 / 321950858) & b) << (int)num4 == 0)
										{
											num4 = num4 / (uint)(~((int)(0 - num4) / ((int)num4 + -256797946))) - 1785435592;
											continue;
										}
										goto IL_0fa1;
									}
									goto IL_148c;
									IL_0a72:
									num4 = 5 + (0 - num4 / 147035593);
									D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[0x3670EB75 ^ ((int)num4 - -913371127)](17 + (ushort)num4, -250755829 ^ (-250755832 | b));
									_671BC22C.BF9F3D1F.FE8E0C9E[(int)(b ^ num4) - -852](ref D6A007B, "Attempt ");
									goto IL_0acf;
									IL_0acf:
									D6A007B.AppendFormatted(_003Cattempt_003E5__6);
									_671BC22C.BF9F3D1F.FE8E0C9E[0x6DAF7CD3 ^ (0x448A7B81 | (num4 ^ ((num4 - num4) ^ 0x29A50586)))](ref D6A007B, ": Error - ");
									if ((uint)((int)num4 / (int)b) % ~(num4 & 0x92320120u) < 2301292473u % (uint)b * 128)
									{
										goto IL_0308_2;
									}
									num4 = (uint)(((int)((uint)((int)num4 * -977904608) ^ num4) / (int)((uint)b / (uint)b) * -1375309780) ^ 0x41EFA680);
									continue;
									IL_10c3:
									num4 = (uint)((312866349 << (int)num4) + 1958739968);
									if (_003Cattempt_003E5__6 == _003CmaxRetries_003E5__4)
									{
										awaiter2 = _671BC22C.BF9F3D1F.FE8E0C9E[1273 + b2 % ((-1314553831 >>> (((int)b2 < (int)num4) ? 1 : 0)) | 0x79265B23)](_671BC22C.BF9F3D1F.FE8E0C9E[793123540 + (((short)b << ((162387645 > b) ? 1 : 0) + (int)(sbyte)b) | -793121639)](result3)).GetAwaiter();
										num3 = (short)(255 >>> ~b);
										if ((int)((num4 << (1016008616 >>> (int)num3)) / (uint)(b2 + ((int)num4 + -820195043))) <= (int)((uint)num3 & ((uint)(~(num3 >> (int)num3)) / (uint)b)))
										{
											num4 = (uint)((num3 | -1335280022) + 1976191437);
											continue;
										}
										goto IL_0d8c;
									}
									goto IL_1812;
									IL_0cd5:
									num4 = (uint)(-156 + (byte)((b * 246429465) | (int)(1731503757u % (uint)(~((int)num4 % (int)b2)))));
									if (num6 != (((int)(0x5862F9C6 & (2268106558u / (uint)(-587121358 + b2))) >> (int)((uint)((int)(num4 % 814307495) % 1297826897) / (uint)(b ^ 0x398B6C1F))) ^ 0x1F4))
									{
										if (((uint)((b2 + b) / ((b == b) ? 1 : 0) % 1661236924) & (0 - (uint)b / (uint)(-13306847 - b2))) >= (((uint)(b2 >>> 19) % num4 << 19 == 2705067796u) ? 1u : 0u))
										{
											num4 = (uint)((~b - 2035451216) ^ -1196201698);
											continue;
										}
										goto IL_0548;
									}
									goto IL_1086;
									IL_077e:
									num4 = (uint)((1696915480 << (int)(num4 | 0xB02C7CB8u)) ^ 0x4CA44C5F);
									if (!awaiter2.IsCompleted)
									{
										goto IL_07a3;
									}
									goto IL_08d2;
									IL_0fe9:
									num4 = (uint)((b2 + b2 >>> 31) ^ 1);
									result2 = awaiter2.GetResult();
									if ((-999085725 | num3) != 0)
									{
										num4 = ((2137513983u < (uint)(-308931063 + (((b ^ num3) < num3) ? 1 : 0))) ? 1u : 0u) + 2836437940u;
										continue;
									}
									goto IL_14fd;
									IL_07a3:
									num = (_003C_003E1__state = -1718005355 + (int)((num4 | ((uint)num2 / 3895156114u) | num4) * (((3499994150u > (uint)num2) ? 1u : 0u) ^ 0x3E9AA915u)));
									_003C_003Eu__2 = awaiter2;
									num4 = (uint)(781581953 >>> num2 / num2 / ~(num2 >>> (int)num4)) % (uint)(sbyte)num4;
									num4 = ((num4 == 2) ? 1u : 0u) + 3099054118u;
									continue;
									IL_14fd:
									_671BC22C.BF9F3D1F.FE8E0C9E[(num4 * 1905814065) ^ 0x68C](_671BC22C.BF9F3D1F.FE8E0C9E[(-1256870518 >>> (int)b) % (-61597696 - num3 / ~num3) + -40701888]("Max retries reached. Last error: ", result4));
									if ((num3 | -964009937) <= (int)(2703242413u / (uint)b + num4) * (-1173243134 + (int)(731149966u / (uint)b)) >>> (int)((uint)(b | -812956393) % (uint)(-190464450 >>> (int)num3) - (uint)(num3 >> 27) % 3776047662u))
									{
										num4 = 3685627571u + (741927201 % ~((b == 798170153) ? 1u : 0u) + 27828971);
										continue;
									}
									goto IL_0612;
									IL_06dd:
									num4 = (uint)((((int)(num4 | 0x6A0EF0B1) % (int)num4) | -1862552102) + -1281077664);
									awaiter2 = _671BC22C.BF9F3D1F.FE8E0C9E[1274 + (int)(num4 / 4294939226u) * (1790488465 % num2)](_671BC22C.BF9F3D1F.FE8E0C9E[-367720336 ^ ((int)((num4 + 1966071231 << 7) | (1780427272 / num4 % (num4 | 0x9CA44AAEu))) >> -1581834870 / (num2 | 0x65241AB0))](result3)).GetAwaiter();
									if (-num2 != 0)
									{
										num4 = (uint)(num2 + -577935739 - -61187450);
										continue;
									}
									goto IL_08d2;
									IL_0eb6:
									num4 = (uint)(1 + (num3 << -2046511432 + b >> 3 >>> (int)b2));
									if (!awaiter2.IsCompleted)
									{
										num = (_003C_003E1__state = 1 + -(-((b ^ num3) << (int)num3)));
										_003C_003Eu__2 = awaiter2;
										num4 = ((((num4 == 714263564) ? 1 : 0) >> (int)num3 == -753216495) ? 1u : 0u);
										if ((uint)(~(b + b)) > (uint)((-1228257031 >>> (int)num4) & (b % 1133235391)))
										{
											num4 = (uint)(-1917730510 + b);
											continue;
										}
										goto end_IL_0308;
									}
									goto IL_0fca;
									IL_0d8c:
									if (num6 != (((int)num4 >> 30 > 1835406806) ? 1 : 0) % -1199365354 - 1033989686 - -1033990188)
									{
										if (((909342106u / (uint)b2) ^ 0xCBBDC21Fu) >> (int)((uint)((int)(num4 % 3064175762u) + -910319558) % ~(1370222350u % (uint)b2)) != 0)
										{
											num4 = (uint)(-1433785200 + b2);
											continue;
										}
										goto IL_0fa1;
									}
									goto IL_1086;
									IL_0631:
									num4 = num4 ^ 0xA60BE301u ^ 0xA18097FDu;
									result3 = awaiter3.GetResult();
									if (_671BC22C.BF9F3D1F.FE8E0C9E[((int)(num4 >> (int)num4) - (690857147 / (int)(num4 << (int)num4) + (int)num4)) ^ -1285836368](result3))
									{
										num2 = 1630165648 >>> (-301267428 >> ((int)num4 >> (int)num4));
										if ((int)(0xA126693Bu & num4) + (short)num2 / ~(num2 >>> 5) != -487225019 >> (int)(sbyte)num2 >> (int)((uint)(num2 >> 5) / (uint)(num2 % 1302514477)))
										{
											num4 = (num4 ^ 0xFFFFFF90u) + 2117918421;
											continue;
										}
										goto IL_08d2;
									}
									num4 = ((4178988178u > (uint)((int)(num4 / 2528840497u % (597477126 + num4)) >> (int)(num4 / num4))) ? 1u : 0u);
									if ((0xCC9EC18Au ^ num4) % 2659473432u != 0)
									{
										b = (byte)(0 + num4);
										if (((4026567698u / num4) ^ 0x79BFEE3B ^ (4169601584u % num4 * num4)) != 0)
										{
											num4 = (uint)(b - -1613958203 - 959337649);
											continue;
										}
										goto default;
									}
									goto IL_0d8c;
									IL_0d7c:
									num4 = (uint)((b | -894796126) ^ -894796126);
									goto IL_0d8c;
									continue;
									end_IL_036b:
									break;
								}
								num4 = (uint)(((-1038594432 << (int)(((num4 > num4) ? 1u : 0u) & num4)) - (int)((num4 >> 28) - num4)) ^ 0x7AD01E9B);
								goto IL_082c;
								IL_0fca:
								if ((short)(~b) != 0)
								{
									num4 = (uint)(0x4591D0BF ^ (0x1CADC9AA | b2));
									goto IL_036b;
								}
								goto end_IL_0308;
								IL_0612:
								num4 = (uint)(-((int)num4 + -732184103));
								num4 = 0xEC504C1Cu ^ (0 - num4 * num4);
								goto IL_036b;
								IL_131e:
								num4 = (num4 ^ ((-2037222756 == ((int)b2 - (int)num4) % -1867458133) ? 1u : 0u)) - 305980730;
								break;
								IL_0c66:
								num4 = (uint)(b * 1487052347 + -1487052346);
								goto IL_0c76;
								IL_0c76:
								num6 = (int)_671BC22C.BF9F3D1F.FE8E0C9E[((int)(0 - num4) * ((int)((uint)((int)num4 % (int)b) & num4) * (0x5A30BE8C ^ ((int)num4 >> 26)))) ^ 0x632](result3);
								b2 = (sbyte)(byte)(num4 % 553837885 % (0 - num4 / b));
								num4 = (uint)(((int)num4 % ~((int)(num4 >> (int)b) * -1089759303)) ^ -679084113);
								goto IL_036b;
								IL_05a4:
								_003C_003Eu__1 = default(TaskAwaiter<HttpResponseMessage>);
								b = (byte)(b * (799404972u / (uint)(~b | -1610612736)));
								if (990640164 / ~((((uint)(num3 % num3) > (uint)b) ? 1 : 0) * (int)num3) != 0)
								{
									num = (_003C_003E1__state = (-1532270403 >> (int)((uint)((1555705730 >>> (int)num3) - 296081411) / (uint)num3)) + 2922);
									num4 = 3741315528u;
									goto IL_0612;
								}
								goto IL_09a6;
								IL_0fa1:
								num = (_003C_003E1__state = 922807310 * b2 + -1);
								num4 = 1u;
								b2 -= -1;
								b = (byte)(b + -110);
								num3 = 0;
								goto IL_0fca;
								IL_0986:
								num4 = (uint)(1084808109 % ~(b / (-586480705 >>> (int)num4))) / 62u + 1285835871;
								goto IL_09a6;
								IL_09a6:
								result = JsonConvert.DeserializeObject<ApiResponse>(item).data;
								if (944740836 - b >= (int)(~num4 >> 25))
								{
									num4 = (uint)(-285128962 + -((int)b % (int)(2224939425u / (uint)b) >> ((1672564488 > num2) ? 1 : 0)));
									goto IL_036b;
								}
								goto IL_148c;
								IL_08d2:
								b = (byte)((int)((uint)(-num2) % num4 << 21) >> 18);
								if ((uint)(b & 0x9EAFCB1) >= (0x5D987CB8 ^ num4))
								{
									break;
								}
								num4 = (uint)(num2 + -307559774);
								goto IL_036b;
								IL_148c:
								result4 = awaiter2.GetResult();
								num4 += num4;
								if (~(b - num3 >> (int)num3) * ((num3 + 1202801965) * ((int)num4 % (int)b) + num3) == 0)
								{
									num4 = (uint)((int)((uint)(num3 * b2) & ((526449113 == b) ? 1u : 0u)) % (int)(~(((num4 & 0xD02E8902u) << (int)b2) / ~num4)) - -1133599006);
									goto IL_036b;
								}
								goto IL_082c;
							}
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
							continue;
							end_IL_0308:
							break;
						}
					}
					catch (Exception fEBDE)
					{
						D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[1165](31, 2);
						while (true)
						{
							IL_15ea:
							_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, "Attempt ");
							D6A007B.AppendFormatted(_003Cattempt_003E5__6);
							while (true)
							{
								IL_160e:
								_671BC22C.BF9F3D1F.FE8E0C9E[852](ref D6A007B, ": Exception occurred - ");
								num2 = 0;
								while (true)
								{
									switch ((uint)num2 % 4u)
									{
									case 1u:
										num2 = -435214024 * b2 * (0x618BDE0E | num2);
										while (_003Cattempt_003E5__6 != _003CmaxRetries_003E5__4)
										{
											if ((int)(0 - 1318507557u % (uint)(~num2)) * -939278791 + 615208077 % ~((int)(2265147535u / (uint)(~b2)) * num2) < (b2 - b2) / -1452456490)
											{
												continue;
											}
											goto IL_17e1;
										}
										goto IL_1732;
									case 2u:
										num2 = (int)(((uint)((-2134989761 << num2 >> 9) + ((b2 == b2) ? 1 : 0)) % (uint)(-255955302 / ~(ushort)b2)) ^ 0xBBB7181);
										goto end_IL_15d3;
									case 3u:
										{
											num2 = (int)(53938 % ((uint)(b2 ^ 0x5F1A9BA9) / 8118u) - 53938);
											goto end_IL_1628;
										}
										IL_17e1:
										num2 = (sbyte)num2 ^ 0x3BA4321F;
										continue;
									}
									_671BC22C.BF9F3D1F.FE8E0C9E[((uint)num2 / (uint)(~num2) % ((uint)(num2 + num2) / 2718733835u + 532283452)) ^ 0x137](ref D6A007B, _671BC22C.BF9F3D1F.FE8E0C9E[2117 + 454723506u % (uint)(562070805 / ~num2) * ((uint)(num2 >> 13) % (uint)(num2 - -1499534320))](fEBDE));
									_671BC22C.BF9F3D1F.FE8E0C9E[-num2 + 1676](_671BC22C.BF9F3D1F.FE8E0C9E[(num2 & -2076612809) + 934](ref D6A007B));
									b2 = (sbyte)(((uint)((0x1A24D29D ^ num2) + num2) / (uint)(~num2)) & 0x362D4DA7);
									if (((1672537890u % (uint)(~b2)) | (uint)num2) == 0)
									{
										goto IL_15ea;
									}
									num2 = -1709996899 - b2 + 1176705952;
									continue;
									IL_1732:
									if ((uint)(-((num2 >>> num2) / (-2119461855 | b2))) % (uint)(((int)((uint)num2 / (uint)(~b2)) >> 3) + -1542145635) == (uint)(934511892 >>> -b2))
									{
										goto IL_160e;
									}
									num2 = b2 - (147 << (int)b2) - 1179971135;
									continue;
									end_IL_1628:
									break;
								}
								break;
							}
							break;
						}
						goto IL_1812;
						end_IL_15d3:;
					}
					goto IL_1cc8;
					IL_1812:
					b = 84;
					b = (byte)(~(b * b) ^ (1 * ((-1322816722 / b) & b)));
					if (((-164454138 ^ b) << (int)b) / ((-1786655343 >>> b % b) * (1 / ~(b >>> 14))) == 0)
					{
						b = (byte)(-(b ^ -537223813) - 537223900);
						goto IL_186b;
					}
					goto IL_1934;
					IL_1cc8:
					b = 253;
					if (1805766422 * b != 0)
					{
						b = (byte)((uint)(b + b + -433437151) / (uint)(~((b ^ b) & ~b)) % b - 4294967238u);
						goto IL_186b;
					}
					goto IL_1c65;
					IL_1ae6:
					num = (_003C_003E1__state = -1);
					b = 74;
					goto IL_1af4;
					IL_1934:
					if (!_671BC22C.BF9F3D1F.FE8E0C9E[(0x6594137 | (b + b / 639539591)) - 106510984](ref awaiter))
					{
						num4 = ((673082636 < (-862535660 ^ (-383802856 + (-182432603 ^ b)))) ? 1u : 0u);
						if ((-1769482749 & b & -2) % (b << 18) != (int)(0x3A9BDA8 ^ num4))
						{
							goto IL_186b;
						}
						goto IL_1ad2;
					}
					goto IL_1af4;
					IL_1ad2:
					awaiter = _003C_003Eu__3;
					_003C_003Eu__3 = default(TaskAwaiter);
					goto IL_1ae6;
					IL_1caf:
					b = (byte)(num3 - -116);
					if (_003Cattempt_003E5__6 <= _003CmaxRetries_003E5__4)
					{
						goto IL_0308;
					}
					goto IL_1cc8;
					end_IL_003b:;
				}
				finally
				{
					num2 = 0;
					while (true)
					{
						IL_1d85:
						switch ((uint)num2 % 3u)
						{
						default:
							if (num < (((-1281251164 | -num2) - 1572187020 > -628752739) ? 1 : 0) - 1)
							{
								num2 = (int)(((uint)(-1062473543 & num2) % 4294967291u) & 0x1B);
								num2 = -721266246 + (int)((uint)((int)((uint)num2 % 1133600158u) % ~num2 * (short)num2) % (uint)(~(num2 >> 31)));
								continue;
							}
							break;
						case 1u:
							num2 = (int)(((((uint)(num2 | (ushort)num2) > (uint)((sbyte)num2 / (num2 * num2))) ? 1u : 0u) % (((uint)num2 % (uint)(~num2)) | (uint)(num2 - num2))) ^ 1);
							while (_003Cclient_003E5__2 != null)
							{
								if (~(num2 / ~num2 / 106141697) != 0)
								{
									num2 = 0x2FB21010 ^ (-205539029 - num2);
									goto IL_1d85;
								}
							}
							break;
						case 2u:
							num2 = (num2 << -num2 + -2110994113 / num2) - 229194496;
							((IDisposable)_003Cclient_003E5__2).Dispose();
							break;
						}
						break;
					}
				}
			}
			catch (Exception exception)
			{
				uint num4 = 4294823382u;
				do
				{
					_003C_003E1__state = (1750 << (int)((0 - num4) | num4)) ^ 0x7FFFFFFE;
					_003C_003Et__builder.SetException(exception);
				}
				while (0u - ((num4 * num4 - 1688425760 == num4) ? 1u : 0u) != 0);
				return;
			}
			do
			{
				_003C_003E1__state = -2;
				b = 242;
			}
			while (b <= ((b == b) ? 1 : 0) * (int)((uint)b % (uint)b));
			_003C_003Et__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			sbyte b = 0;
			if ((-354 ^ b) != 0)
			{
				do
				{
					_003C_003Et__builder.SetStateMachine(stateMachine);
				}
				while (b > (0 & b));
			}
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[AsyncStateMachine(typeof(_003CGetNearbyPoiFromApi_003Ed__0))]
	public static Task<NearbyPoi> GetNearbyPoiFromApi(int poiId, int x, int y, string direction)
	{
		_003CGetNearbyPoiFromApi_003Ed__0 stateMachine = default(_003CGetNearbyPoiFromApi_003Ed__0);
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder<NearbyPoi>.Create();
		int num = -62870287;
		if ((int)(1746456339u % (uint)(-567193576 / num)) / (int)(byte)(0x5B2E8437 & ~num) <= num - -1642644323)
		{
			num = 1143073927 + num - -1471047835;
			goto IL_0043;
		}
		goto IL_0156;
		IL_0156:
		do
		{
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			num = num * num >>> (int)((uint)(((num == num) ? 1 : 0) - (-981403745 >>> num)) % (uint)(~num));
		}
		while ((int)((uint)(num * num) / (uint)(~(-1373080662 & (num << 22)))) >> num != 0);
		num = (-8866844 * num) ^ 0x7931EE0B;
		goto IL_0043;
		IL_0043:
		while (true)
		{
			switch ((uint)num % 5u)
			{
			default:
				num ^= 0x6451AEE2;
				stateMachine.poiId = poiId;
				num <<= -510617031 + num;
				num = -(120118438 - num) - 1333332904;
				continue;
			case 1u:
				num = 0x6C148000 ^ (((num >>> num) & num) * (-2003234142 * (int)(612783903u / (uint)num) + -411145140));
				stateMachine.x = x;
				stateMachine.y = y;
				goto IL_00c3;
			case 2u:
				num -= num;
				stateMachine._003C_003E1__state = -1 ^ (0x38186A9F & num);
				if ((num | ((num ^ (1437024057 - num)) << num)) != 0)
				{
					num = (int)(~((uint)(-1038208381 | (num & 0x2680C3A3)) % 3156648494u) ^ 0x79123EA5);
					continue;
				}
				goto IL_00c3;
			case 3u:
				break;
			case 4u:
				{
					num = 2101346304 + (num << 22);
					return stateMachine._003C_003Et__builder.Task;
				}
				IL_00c3:
				stateMachine.direction = direction;
				num = (int)(0u / (uint)(~(133 * ((num < (num & num)) ? 1 : 0))));
				num = (1 << -451485544 % (int)(((num == num) ? 1u : 0u) << -551026407 % ~num)) + 211564701;
				continue;
			}
			break;
		}
		num = (num / -749310078) ^ 2;
		goto IL_0156;
	}

	public static void UploadMissingPoi(int poiId, int x, int y, string direction, string type)
	{
		sbyte b = 119;
		HttpClient httpClient = _671BC22C.BF9F3D1F.FE8E0C9E[((b % b + b) | (b >>> (int)b)) - -479]();
		try
		{
			uint num = (uint)(-22763674 - b);
			string _070E63A = MapDirection(direction);
			b = (sbyte)(~(~(b * (b >> 4))));
			StringContent fE8B089A = _671BC22C.BF9F3D1F.FE8E0C9E[0 - (((int)num > (int)b) ? 1 : 0) - -1592](_671BC22C.BF9F3D1F.FE8E0C9E[0xCFBDF996u ^ (0xCFBDFF8Du | ((1415396864 / (102493340 >> (int)b) < (int)num) ? 1u : 0u))](new global::AF06D8B9<int, int, int, string, string, string>(poiId, x, y, _070E63A, _671BC22C.BF9F3D1F.FE8E0C9E[num * 565068442 + 410516565](D134E92A.A40E730E), type)), _671BC22C.BF9F3D1F.FE8E0C9E[(-63633466 ^ b) + 63633765](), "application/json");
			try
			{
				if ((((int)((uint)(-628506857 % (int)num) | num) > 3864504) ? 1 : 0) % 1402136595 == 0)
				{
					b = (sbyte)(45 + (num & 0) / (1974819328u % (uint)(-1977530493 | b) << (int)(((1854001796 < (int)num) ? 1u : 0u) % 1204287687u)));
					goto IL_0110;
				}
				goto IL_01b4;
				IL_01b4:
				HttpResponseMessage result = default(HttpResponseMessage);
				while (_671BC22C.BF9F3D1F.FE8E0C9E[595 + ~b](result))
				{
					if (((int)b - (int)(~(num - 1017539744))) / (int)num >= (int)(0u / (uint)(~((int)((uint)b / 3149618071u) % (int)b))) % (int)b)
					{
						continue;
					}
					goto IL_0110;
				}
				goto IL_0298;
				IL_0110:
				string item = default(string);
				DefaultInterpolatedStringHandler D6A007B = default(DefaultInterpolatedStringHandler);
				uint num2 = default(uint);
				while (true)
				{
					switch ((uint)b % 9u)
					{
					default:
						b = (sbyte)(~(28805 << (-651770356 << (int)(~num))) - -28871);
						result = _671BC22C.BF9F3D1F.FE8E0C9E[0x15B5F2A ^ (0 - num)](httpClient, _8E94B429._751D7C9F, fE8B089A).Result;
						num = (uint)(1478379553 << (int)b);
						b = (sbyte)(-1068949333 ^ (int)((num | (uint)b) + (num & (uint)(-1566628209 >>> (int)b))));
						continue;
					case 1u:
						break;
					case 2u:
						goto IL_01f5;
					case 3u:
						b = (sbyte)(0x62C8DEE3 ^ (0xD2F44A42u ^ (1182 + num)));
						JsonConvert.DeserializeObject<ApiResponse>(item);
						return;
					case 4u:
						b = (sbyte)((int)((288251144 / ~(num / ~((688260541u < (uint)b) ? 1u : 0u))) ^ (((int)num < 2105686904) ? 1u : 0u)) ^ -27);
						D6A007B = _671BC22C.BF9F3D1F.FE8E0C9E[338636450 * ((int)(((uint)b % num) & num) % (int)((uint)b % 656436533u)) - 677271735]((int)(~num) + -1338208182, -552852601 * (int)num + -2025920717);
						b = (sbyte)((int)num >> (int)num);
						b = (sbyte)(-95531019 + (b + 95531008));
						continue;
					case 5u:
						b += 11;
						_671BC22C.BF9F3D1F.FE8E0C9E[(int)num - -1338209042](ref D6A007B, "Error: ");
						num2 = (uint)(1825421526 >> (-954121285 >> ((b != 0) ? 1 : 0)));
						b = (sbyte)((int)(0 - ((num2 | 0xA22BE2A0u) - 1159518085)) - (int)b + 1561267014);
						continue;
					case 6u:
						b = (sbyte)((-1768082644 & ~(-946509894 ^ (sbyte)num)) - 268963828);
						D6A007B.AppendFormatted(_671BC22C.BF9F3D1F.FE8E0C9E[b - -1570](result));
						b = (sbyte)((int)(177941159u / (uint)b) % (int)(~(num2 ^ num2)));
						if ((byte)(b + -1543204066) == 0)
						{
							return;
						}
						b = (sbyte)(-96234352 ^ ((96234368 + ((2651909679u < num2) ? 1 : 0)) / (int)(~((b == -1078451424) ? 1u : 0u)) - (b >>> (int)num2)));
						continue;
					case 7u:
						b = (sbyte)(-14 + (int)(2 * (1318806307u % (uint)(b - 6))));
						do
						{
							_671BC22C.BF9F3D1F.FE8E0C9E[84121123 + 524800 / ~b + -83594647](_671BC22C.BF9F3D1F.FE8E0C9E[(((num / (uint)(~b) == (uint)b) ? 1 : 0) % ~(b / 1813471412 % (int)num)) ^ 0x3A6](ref D6A007B));
						}
						while (((uint)((b >>> 5) + b - (int)b * (int)num2) & (353788599 + (num * 1217703172 >> 26))) != 0);
						b = (sbyte)((int)((num | (uint)b | (0 - num2)) & (uint)((int)(num + 1652909240 >> 28) >> (int)num2 % (int)(num2 | 0xCCBF0709u))) - -44);
						continue;
					case 8u:
						b = (sbyte)((((sbyte)(1689584280 + num) > b) ? 1 : 0) >> (int)(0 - num));
						_671BC22C.BF9F3D1F.FE8E0C9E[3282438300u / (uint)(0 - (((int)num2 > (int)num) ? 1 : 0) - b) + 1676](_671BC22C.BF9F3D1F.FE8E0C9E[(((int)num2 > (int)b) ? 1 : 0) + 1273](_671BC22C.BF9F3D1F.FE8E0C9E[-1029288212 ^ (((int)(num2 << 10) >> ((num2 < 1268301861) ? 1 : 0)) - (short)(0x6CBDF705 ^ num) + (int)(((uint)b / num2) ^ (0x729A3AA6 ^ num)))](result)).Result);
						b -= -65;
						return;
					}
					break;
					IL_01f5:
					item = CE0A9D0F.AFB39003(_671BC22C.BF9F3D1F.FE8E0C9E[0x5FE76960 ^ (0x5AA54D9A | ((-721033564 << (int)b) * (int)num))](_671BC22C.BF9F3D1F.FE8E0C9E[b - -1840](result)).Result).data;
					if ((uint)b + (0 - 3626480295u % (uint)b + 943344051) != 0)
					{
						b = (sbyte)(-19713 - (int)(num - ((uint)b ^ (num | num))) + 19788);
						continue;
					}
					goto IL_0298;
				}
				b = (sbyte)(-1338208253 ^ (int)num);
				goto IL_01b4;
				IL_0298:
				b = (sbyte)(num ^ 0x449143A6);
				b = (sbyte)((int)num * -1786414450 - -90081182);
				goto IL_0110;
			}
			catch (Exception fEBDE)
			{
				_671BC22C.BF9F3D1F.FE8E0C9E[1676](_671BC22C.BF9F3D1F.FE8E0C9E[1797]("Exception occurred: ", _671BC22C.BF9F3D1F.FE8E0C9E[2117](fEBDE)));
			}
		}
		finally
		{
			if (httpClient != null)
			{
				b = 0;
				((IDisposable)httpClient).Dispose();
			}
		}
	}

	public static string MapDirection(string direction)
	{
		string result = default(string);
		byte b2 = default(byte);
		byte b;
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](direction, "North"))
		{
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[720](direction, "West"))
			{
				b = 153;
				if (0 - (((uint)(~b) / 1546607805u) & 0xB93B2C3Au) != 0)
				{
					b = (byte)(((uint)(b % -737714759) % (uint)(1096670043 + (b ^ 0x400C7120))) ^ 0x1C);
					goto IL_006f;
				}
				goto IL_02eb;
			}
			result = "west";
			b2 = 0;
			if (-710564722 >>> (int)((uint)b2 / (uint)(~b2)) == ((-257889644 > b2 * b2) ? ((sbyte)1) : ((sbyte)0)))
			{
			}
		}
		else
		{
			b2 = 0;
			result = "north";
		}
		goto IL_0309;
		IL_006f:
		uint num = default(uint);
		ushort num2 = default(ushort);
		while (true)
		{
			switch ((uint)b % 7u)
			{
			case 1u:
				goto end_IL_006f;
			case 2u:
				b = (byte)(((((b - b == (int)(~num)) ? 1u : 0u) < num % 817673766) ? 1 : 0) * (1930750006 / (int)(0 - num)) + -12537184);
				result = "east";
				b2 = 0;
				b = (byte)(0x21 ^ (b2 ^ 0x30));
				continue;
			case 3u:
				goto IL_021f;
			case 4u:
				goto IL_0279;
			case 5u:
				goto IL_02c2;
			case 6u:
				goto IL_02fd;
			}
			b = (byte)((b % 1597924104) ^ 0x1C);
			goto IL_00a2;
			IL_02fd:
			result = "unknown";
			b2 = (byte)(b2 + -78);
			goto IL_0309;
			IL_02c2:
			b = (byte)(1118800937 + (-1143666873 << (int)(num2 % ~((-1129758193 == (int)((uint)b / 1278682421u)) ? 1u : 0u))));
			goto IL_0309;
			IL_0279:
			b = (byte)(0 - 3030995496u / (uint)(num2 ^ b2) - 4222800584u);
			result = "south";
			b2 ^= 0x4E;
			if (b + 1663584529 != 0)
			{
				b = (byte)(1761188562 + (-1761188431 - (int)(0 / (2402376124u / (uint)num2))));
				continue;
			}
			goto IL_00a2;
			IL_00a2:
			if (!_671BC22C.BF9F3D1F.FE8E0C9E[(0x38B30609 & (b + 294887977 >> 6)) - 131897](direction, "East"))
			{
				b2 = (byte)((204470275 >>> (int)b) - -919286856);
				if ((((uint)b2 % 4121786276u) | (uint)((int)(((uint)b / (uint)b) | ((740400771 == b) ? 1u : 0u)) * ((int)(3080548256u / (uint)b2) >> -535659849 - b))) != 0)
				{
					b = (byte)(((((b | 0x15AD583F) ^ 0x4E9C6616) | b2) >> (-1417585224 + b2) * 1222574040) ^ 0x5B4E);
					continue;
				}
				goto IL_0309;
			}
			num = (uint)(~b);
			b = (byte)(((1166361602 >> (int)b) & -b) ^ 7);
			continue;
			IL_021f:
			b = (byte)(0x883084BBu ^ (((uint)(-936334439 - (int)num) % (uint)(-b)) & 0xBF3786A2u));
			goto IL_0309;
			continue;
			end_IL_006f:
			break;
		}
		b = (byte)(153 + (ushort)(10109320u / (uint)(b * b2 << (int)b2)));
		goto IL_015b;
		IL_02eb:
		if (706730970 * b2 != 0)
		{
			goto IL_006f;
		}
		goto IL_015b;
		IL_015b:
		if (!_671BC22C.BF9F3D1F.FE8E0C9E[0 / (short)(-1665591649 % (int)(640044050u / (uint)b)) - -720](direction, "South"))
		{
			goto IL_02eb;
		}
		num2 = (ushort)((uint)(-1364965839 % (int)(2559880622u % (uint)b2)) % (uint)b);
		b = (byte)((-1248882807 >> (int)b2) * (b2 - (b2 << -1650973046 % b)) - -930789846);
		goto IL_006f;
		IL_0309:
		return result;
	}
}
