using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using OKHOSTING.Core.Extensions;

namespace OKHOSTING.Core.Data
{
	/// <summary>
	/// Defines methods for converting objects from one Type to another,
	/// as well as serialization and deserialization methods
	/// </summary>
	public static class Converter
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
			if (value == null || value == DBNull.Value) return null;

			//no need for conversion
			if (conversionType.IsAssignableFrom(value.GetType())) return value;

			//from string to object
			if (value is string) return ToObject((string)value, conversionType);

			//from object to TimeSpan
			if (conversionType.Equals(typeof(TimeSpan))) return ToTimeSpan(value);

			//from object to string
			if (conversionType.Equals(typeof(string))) return ToString(value);

			//from object to enumeration
			if (conversionType.IsEnum) return ToEnum(value, conversionType);

			//Trying to convert throught IConvertible interface
			return Convert.ChangeType(value, conversionType);
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
			return ToTimeSpan((long)Convert.ChangeType(ticks, typeof(long)));
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
				return ToString((System.Enum)value);

			else if (value is IXmlSerializable)
				return ToString((IXmlSerializable)value);

			else if (value is IEnumerable)
				return ToString((IEnumerable)value);

			else if (value is DateTime)
				return ToString((DateTime)value);

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
			
			return Convert.ChangeType(value, value.GetTypeCode()).ToString();
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
		/// Returns the string representation of the specified NameValueCollection
		/// </summary>
		/// <param name="values">List that will be parsed as a string</param>
		/// <returns>
		/// The properties stored on values on the format
		/// Property1=ValueOfProperty1&Property2=ValueOfProperty2...
		/// </returns>
		public static string ToString(NameValueCollection value)
		{
			//Local Vars
			string nameValues = string.Empty;

			//Crossing the name / value pairs 
			foreach (string name in value.AllKeys)
			{
				nameValues += name + "=" + value[name] + "&";
			}

			//Remove last &
			nameValues = nameValues.TrimEnd('&');

			//Returning the string to the caller
			return nameValues;
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
			if (conversiontype.IsAssignableFrom(value.GetType())) return value;

			//TimeSpan
			if (conversiontype.Equals(typeof(TimeSpan))) return ToTimeSpan(value);

			//DateTime
			if (conversiontype.Equals(typeof(DateTime))) return ToDateTime(value); 
			
			//enum
			if (conversiontype.IsEnum) return ToEnum(value, conversiontype);

			//IStringSerializable
			//if (conversiontype.GetInterface(typeof(IStringSerializable).FullName) != null) return ToIStringSerializable(value, conversiontype);

			//IXmlSerializable
			if (conversiontype.GetInterface(typeof(IXmlSerializable).FullName) != null) return ToIXmlSerializable(value, conversiontype);

			//IEnumerable
			if (conversiontype.GetInterface(typeof(System.Collections.IEnumerable).FullName) != null) return ToIEnumerable(value, conversiontype);

			//generic
			return Convert.ChangeType(value, conversiontype);
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
			IStringSerializable result = (IStringSerializable)conversiontype.CreateInstance();
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
		/// Creates a NameValueCollection filed with the values of the string
		/// </summary>
		/// <param name="value">
		/// String which will be atomized and represented in the NameValueCollection.
		/// Format must be Key1=Value1&Key2=Value2&Key3=Value3...
		/// </param>
		/// <returns>
		/// NameValueCollection filed with the atomized values of the string
		/// </returns>
		public static NameValueCollection ToNameValues(string queryString)
		{
			//null or empty values
			if (string.IsNullOrWhiteSpace(queryString)) return null;

			//split value by & characters to get a string of Key=Valye pairs
			//example: { "Key1=Value1", "Key2=Value2", "Key3=Value3" }
			string[] pairs = queryString.Split('&');

			//create collection
			NameValueCollection result = new NameValueCollection();

			//separate each key and value
			foreach (string pair in pairs)
			{
				string key, val;
				int equalSymbol = pair.IndexOf('=');

				//separate key and value by the = character
				//key = pair.Split('=')[0];
				//val = pair.Split('=')[1];
				key = pair.Substring(0, equalSymbol);
				val = pair.Substring(equalSymbol + 1);

				//add pair to collection
				result.Add(key, val);
			}

			//return result
			return result;
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
		/// Converts a Assembly string representantion into an actual Assembly instance
		/// </summary>
		/// <param name="value">
		/// Value to be converted to Assembly
		/// </param>
		/// <returns>
		/// A Assembly object deserialized from the string
		/// </returns>
		public static Assembly ToAssembly(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return null;

			return Assembly.LoadFrom
			(
				System.AppDomain.CurrentDomain.RelativeSearchPath + value
			);
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