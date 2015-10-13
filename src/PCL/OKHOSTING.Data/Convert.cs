using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

namespace OKHOSTING.Data
{
	/// <summary>
	/// Defines methods for converting objects from one Type to another,
	/// as well as serialization and deserialization methods
	/// </summary>
	public static class Convert
	{
		#region From object to object

		/// <summary>
		/// This method try to convert the specified source value on the 
		/// indicated Type. This class implements converting methods for internal use, 
		/// use it to convert values from database to objetc instance and viceversa,
		/// as well as creating URLs for datatypes, dataobjects or datamemebrers,
		/// or string representations of objects
		/// </summary>
		/// <param name="sourceValue">
		/// Value that you desire to convert
		/// </param>
		/// <param name="conversionType">
		/// Destiny type
		/// </param>
		/// <returns>
		/// The reference to the converted object
		/// </returns>
		public static object ChangeType(object value, Type conversionType)
		{
			//null values
			if (value == null) return null;

			//no need for conversion
			if (conversionType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo())) return value;

			//from string to object
			if (value is string) return ToObject((string)value, conversionType);

			//from object to TimeSpan
			if (conversionType.Equals(typeof(TimeSpan))) return ToTimeSpan(value);

			//from object to string
			if (conversionType.Equals(typeof(string))) return ToString(value);

			//from object to enumeration
			if (conversionType.GetTypeInfo().IsEnum) return ToEnum(value, conversionType);

			//Trying to convert throught IConvertible interface
			return System.Convert.ChangeType(value, conversionType);
		}

		/// <summary>
		/// This method try to convert the specified source value on the 
		/// indicated Type. This class implements converting methods for internal use, 
		/// use it to convert values from database to objetc instance and viceversa,
		/// as well as creating URLs for datatypes, dataobjects or datamemebrers,
		/// or string representations of objects
		/// </summary>
		/// <param name="sourceValue">
		/// Value that you desire to convert
		/// </param>
		/// <returns>
		/// The reference to the converted object
		/// </returns>
		public static TTo ChangeType<TFrom, TTo>(TFrom value)
		{
			return (TTo) ChangeType(value, typeof(TTo));
		}

		/// <summary>
		/// This method try to convert the specified source value on the 
		/// indicated Type. This class implements converting methods for internal use, 
		/// use it to convert values from database to objetc instance and viceversa,
		/// as well as creating URLs for datatypes, dataobjects or datamemebrers,
		/// or string representations of objects
		/// </summary>
		/// <param name="sourceValue">
		/// Value that you desire to convert
		/// </param>
		/// <returns>
		/// The reference to the converted object
		/// </returns>
		public static TTo ChangeType<TTo>(object value)
		{
			return (TTo) ChangeType(value, typeof(TTo));
		}

		/// <summary>
		/// Converts a Enum object representantion into an actual Enum instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to Enum
		/// </param>
		/// <returns>
		/// A Enum deserialized from the object
		/// </returns>
		public static Enum ToEnum(object value, Type enumType)
		{
			//validate arguments
			if (value == null) return null;
			if (enumType == null) throw new ArgumentNullException("enumType");

			//Parsing a string...
			if (value is string) return ToEnum((string)value, enumType);
			
			//parsing an object
			return (Enum)System.Enum.ToObject(enumType, value);
		}

		/// <summary>
		/// Converts a string value containing a timespan into a TimeSpan object
		/// </summary>
		/// <param name="value">
		/// String value to be converted to TimeSpan
		/// </param>
		/// <returns>
		/// A TimeSpan obtained from value
		/// </returns>
		public static TimeSpan ToTimeSpan(string value)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return TimeSpan.Zero;

			//converto to TimeSpan from ticks
			return TimeSpan.Parse(value);
		}

		/// <summary>
		/// Converts a long value containing Ticks into a TimeSpan
		/// </summary>
		/// <param name="value">
		/// Value to be converted to TimeSpan
		/// </param>
		/// <returns>
		/// A TimeSpan obtained from the number of ticks
		/// </returns>
		public static TimeSpan ToTimeSpan(object ticks)
		{
			//validate arguments
			if (ticks == null) return TimeSpan.Zero;

			//converto to TimeSpan from ticks
			return ToTimeSpan((long)System.Convert.ChangeType(ticks, typeof(long)));
		}

		/// <summary>
		/// Converts a long value containing Ticks into a TimeSpan
		/// </summary>
		/// <param name="value">
		/// Value to be converted to TimeSpan
		/// </param>
		/// <returns>
		/// A TimeSpan obtained from the number of ticks
		/// </returns>
		public static TimeSpan ToTimeSpan(long ticks)
		{
			//validate arguments
			if (ticks <= 0) return TimeSpan.Zero;

			//converto to TimeSpan from ticks
			return TimeSpan.FromTicks(ticks);
		}

		#endregion
		
		#region From object to string, string serialization

		/// <summary>
		/// Converts a value to it's string representantion, so it can be deserialized back in the future
		/// </summary>
		/// <param name="value">
		/// Value to be serialized as string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(object value)
		{
			if (value == null) 
				return null;

			else if (value is string)
				return (string) value;

			else if (value is System.Enum)
				return ToString((System.Enum) value);

			else if (value is IXmlSerializable)
				return ToString((IXmlSerializable) value);

			else if (value is IEnumerable)
				return ToString((IEnumerable) value);

			else if (value is DateTime)
				return ToString((DateTime) value);

			return value.ToString();
		}

		/// <summary>
		/// Converts a value to it's database-specific string representantion, so it can be included in a SQL script
		/// </summary>
		/// <param name="value">
		/// Value to be converted to string
		/// </param>
		/// <returns>
		/// A database-specific string representation of value
		/// </returns>
		public static string ToString(Enum value)
		{
			//null values
			if (value == null) return null;

			//return System.Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())).ToString();
			return value.ToString();
		}

		/// <summary>
		/// Converts a value to it's string representantion
		/// </summary>
		/// <param name="value">
		/// Value to be converted to string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(IStringSerializable value)
		{
			//null values
			if (value == null) return null;
			
			return value.SerializeToString();
		}

		/// <summary>
		/// Converts a value to it's string representantion
		/// </summary>
		/// <param name="value">
		/// Value to be converted to string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(IXmlSerializable value)
		{
			//null values
			if (value == null) return null;

			//Creating Serializer of corresponding type and Serializing
			XmlSerializer serializer = new XmlSerializer(value.GetType());
			StringWriter writer = new StringWriter();
			serializer.Serialize(writer, value);

			//Return xml code
			return writer.ToString();
		}

		/// <summary>
		/// Converts a value to it's string representantion, so it can be included in a SQL script
		/// </summary>
		/// <param name="value">
		/// Value to be converted to string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(IEnumerable value)
		{
			//null values
			if (value == null) return null;

			//Creating Serializer of corresponding type and Serializing
			XmlSerializer serializer = new XmlSerializer(value.GetType());
			StringWriter writer = new StringWriter();
			serializer.Serialize(writer, value);

			//Return xml code
			return writer.ToString();
		}

		/// <summary>
		/// Converts a value to it's string representantion
		/// </summary>
		/// <param name="value">
		/// Assembly to be converted to string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(Assembly value)
		{
			//null values
			if (value == null) return null;

			return value.FullName;
		}

		/// <summary>
		/// Converts a value to it's string representantion
		/// </summary>
		/// <param name="value">
		/// Assembly to be converted to string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static string ToString(DateTime value)
		{
			//null values
			if (value == null) return null;

			return value.ToString("yyyy/MM/dd HH:mm:ss");
		}

		#endregion

		#region From string to object

		/// <summary>
		/// Converts a value to it's string representantion, so it can be deserialized back in the future
		/// </summary>
		/// <param name="value">
		/// Value to be serialized as string
		/// </param>
		/// <returns>
		/// A string representation of value
		/// </returns>
		public static object ToObject(string value, Type conversiontype)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (conversiontype == null) throw new ArgumentNullException("conversiontype");

			//no need for conversion
			if (conversiontype.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo())) return value;

			//TimeSpan
			if (conversiontype.Equals(typeof(TimeSpan))) return ToTimeSpan(value);

			//DateTime
			if (conversiontype.Equals(typeof(DateTime))) return ToDateTime(value); 
			
			//enum
			if (conversiontype.GetTypeInfo().IsEnum) return ToEnum(value, conversiontype);

			//IStringSerializable
			//if (conversiontype.GetInterface(typeof(IStringSerializable).FullName) != null) return ToIStringSerializable(value, conversiontype);

			//IXmlSerializable
			if (conversiontype.GetTypeInfo().ImplementedInterfaces.Where(i=> i == typeof(IXmlSerializable)).Count() > 0) return ToIXmlSerializable(value, conversiontype);

			//IEnumerable
			if (conversiontype.GetTypeInfo().ImplementedInterfaces.Where(i => i == typeof(System.Collections.IEnumerable)).Count() > 0) return ToIEnumerable(value, conversiontype);

			//generic
			return System.Convert.ChangeType(value, conversiontype);
		}

		/// <summary>
		/// Converts a Enum string representantion into an actual Enum instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to Enum
		/// </param>
		/// <returns>
		/// A Enum object deserialized from the string
		/// </returns>
		public static Enum ToEnum(string value, Type enumType)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (enumType == null) throw new ArgumentNullException("enumType");

			//Parsing the string...
			return (Enum) System.Enum.Parse(enumType, value);
		}

		/// <summary>
		/// Converts a IStringSerializable string representantion into an actual IStringSerializable instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to IStringSerializable
		/// </param>
		/// <returns>
		/// A IStringSerializable object deserialized from the string
		/// </returns>
		public static IStringSerializable ToIStringSerializable(string value, Type conversiontype)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (conversiontype == null) throw new ArgumentNullException("conversiontype");

			//Deserializing the source value to destiny type
			IStringSerializable result = (IStringSerializable) Activator.CreateInstance(conversiontype);
			((IStringSerializable)result).DeserializeFromString(value);

			return result;
		}

		/// <summary>
		/// Converts a IXmlSerializable string representantion into an actual IXmlSerializable instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to IXmlSerializable
		/// </param>
		/// <returns>
		/// A IXmlSerializable object deserialized from the string
		/// </returns>
		public static IXmlSerializable ToIXmlSerializable(string value, Type conversiontype)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (conversiontype == null) throw new ArgumentNullException("conversiontype");

			//Creating serializer with the destiny type
			XmlSerializer serializer = new XmlSerializer(conversiontype);

			//Creating reader with the source value string representation
			System.IO.StringReader reader = new System.IO.StringReader(value);

			//Deserializing the source value to destiny type
			return (IXmlSerializable) serializer.Deserialize(reader);
		}

		/// <summary>
		/// Converts a IEnumerable string representantion into an actual IEnumerable instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to IEnumerable
		/// </param>
		/// <returns>
		/// A IEnumerable object deserialized from the string
		/// </returns>
		public static IEnumerable ToIEnumerable(string value, Type conversiontype)
		{
			//validate arguments
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (conversiontype == null) throw new ArgumentNullException("conversiontype");

			//try to unparse as xml
			return (IEnumerable) ToIXmlSerializable(value, conversiontype);
		}

		/// <summary>
		/// Gets the value of a key inside a string
		/// </summary>
		/// <param name="values">String that contains key value pairs. Must be formatted as Key1=Value1&Key2=Value2&Key3=Value3...</param>
		/// <param name="key">Key which value will be obtained from the string</param>
		/// <returns>Value of the specified key</returns>
		public static string GetValueFromQueryString(string queryString, string key)
		{
			//Validating arguments
			if (string.IsNullOrWhiteSpace(queryString)) throw new ArgumentNullException("queryString");
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

			int i = queryString.IndexOf(key);

			if (i != -1)
			{
				int j = queryString.IndexOf('=', i);
				int k = queryString.IndexOf('&', j);

				if (j != -1)
				{
					if (k != -1)
					{
						return queryString.Substring(j + 1, k - (j + 1));
					}
					else
					{
						return queryString.Substring(j + 1);
					}
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Converts a DateTime string representantion into an actual DateTime instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to DateTime
		/// </param>
		/// <returns>
		/// A DateTime object deserialized from the string
		/// </returns>
		public static DateTime? ToDateTime(string value)
		{
			DateTime result;

			if (string.IsNullOrWhiteSpace(value)) return null;

			if (DateTime.TryParseExact(value, "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result))
			{
				return result;
			}

			return result;
		}

		#endregion
	}
}