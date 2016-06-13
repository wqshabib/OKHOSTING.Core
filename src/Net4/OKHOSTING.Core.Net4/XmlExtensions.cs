﻿using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// This class publish several extension methods for convert between 
    /// XElement to XmlElement and viceversa, and from XDocument to XmlDocument
    /// and viceversa. The idea is interact simply between the xml object versions 
    /// that supports LINQ and the basic xml objects
    /// <para xml:lang="es">
    /// Esta clase publicar varios métodos de extensión para convertir entre XElement
    /// a XmlElement y viceversa, y desde XDocument a XmlDocument y viceversa. La idea
    /// es simplemente interactuar entre las versiones objeto XML que soporta LINQ y
    /// los objetos XML básicos
    /// </para>
    /// </summary>
    /// <remarks>
    /// Downloaded from http://codepaste.net/jkujfk
    /// <para xml:lang="es">
    /// Descargado desde http://codepaste.net/jkujfk
    /// </para>
    /// </remarks>
    public static class XmlExtensions
	{

        #region XmlTranslator Class (For Internal Use)

        /// <summary>
        /// Utility class that performs the conversions between xml objects
        /// <para xml:lang="es">
        /// clase de utilidad que realiza las conversiones entre los objetos XML
        /// </para>
        /// </summary>
        private class XmlTranslator
		{

			#region Private Fields

			/// <summary>
			/// Auxiliar internal StringBuilder object that is used 
			/// for read and write xml on conversions
			/// </summary>
			private readonly StringBuilder _xmlTextBuilder;

			/// <summary>
			/// Auxiliar internal XmlWriter object that is used 
			/// for stores xml on conversions
			/// </summary>
			private readonly XmlWriter _writer;

			#endregion

			#region Constructors

			/// <summary>
			/// Private constructor (called for remainder constructors)
			/// </summary>
			private XmlTranslator()
			{
				//Loading internal StringBuilder
				_xmlTextBuilder = new StringBuilder();

				//Loading internal XmlTextWriter and configurating it
				_writer = new XmlTextWriter(new StringWriter(_xmlTextBuilder))
				{
					Formatting = Formatting.Indented,
					Indentation = 2
				};
			}

			/// <summary>
			/// Constructs the object from a XNode, writing it content to internal XmlWriter
			/// </summary>
			/// <param name="e">
			/// XNode used for initialize the class
			/// </param>
			public XmlTranslator(XNode e) : this()
			{ e.WriteTo(_writer); }

			/// <summary>
			/// Constructs the object from a XmlNode, writing it content to internal XmlWriter
			/// </summary>
			/// <param name="e">
			/// XmlNode used for initialize the class
			/// </param>
			public XmlTranslator(XmlNode e) : this()
			{ e.WriteTo(_writer); }

			#endregion

			#region Conversion Methods

			/// <summary>
			/// Creates a XElement from XmlTranslator content
			/// </summary>
			/// <returns>
			/// XElement from XmlTranslator content
			/// </returns>
			public XElement CreateXElement()
			{ return XElement.Load(new StringReader(_xmlTextBuilder.ToString())); }

			/// <summary>
			/// Creates a XDocument from XmlTranslator content
			/// </summary>
			/// <returns>
			/// XDocument from XmlTranslator content
			/// </returns>
			public XDocument CreateXDocument()
			{ return XDocument.Load(new StringReader(_xmlTextBuilder.ToString())); }

			/// <summary>
			/// Creates a XmlElement from XmlTranslator content
			/// </summary>
			/// <returns>
			/// XmlElement from XmlTranslator content
			/// </returns>
			public XmlElement CreateXmlElement()
			{ return CreateXmlDocument().DocumentElement; }

			/// <summary>
			/// Creates a XmlDocument from XmlTranslator content
			/// </summary>
			/// <returns>
			/// XmlDocument from XmlTranslator content
			/// </returns>
			public XmlDocument CreateXmlDocument()
			{
				//Creating XmlDocument and returning it
				var doc = new XmlDocument();
				doc.Load(new XmlTextReader(new StringReader(_xmlTextBuilder.ToString())));
				return doc;
			}

			#endregion

		}

        #endregion

        /// <summary>
        /// Converts the specified XElement to XmlElement
        /// <para xml:lang="es">
        /// Convierte el XElement especificada para XmlElement
        /// </para>
        /// </summary>
        /// <param name="xElement">
        /// XElement instance to convert
        /// <para xml:lang="es">
        /// XElement ejemplo para convertir
        /// </para>
        /// </param>
        /// <returns>
        /// XmlElement representation of XElement
        /// <para xml:lang="es">
        /// XmlElement representación de XElement
        /// </para>
        /// </returns>
        public static XmlElement ToXmlElement(this XElement xElement)
		{ return new XmlTranslator(xElement).CreateXmlElement(); }

        /// <summary>
        /// Converts the specified XDocument to XmlDocument
        /// <para xml:lang="es">
        /// Convierte el XDocument especificada a XmlDocument
        /// </para>
        /// </summary>
        /// <param name="xDocument">
        /// XDocument instance to convert
        /// <para xml:lang="es">
        /// XDocument ejemplo para convertir
        /// </para>
        /// </param>
        /// <returns>
        /// XmlDocument representation of XDocument
        /// <para xml:lang="es">
        /// Xml representación Documento de XDocument
        /// </para>
        /// </returns>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
		{ return new XmlTranslator(xDocument).CreateXmlDocument(); }

        /// <summary>
        /// Converts the specified XmlElement to XElement
        /// <para xml:lang="es">
        /// Convierte el XmlElement especificada para XElement
        /// </para>
        /// </summary>
        /// <param name="xmlElement">
        /// XmlElement instance to convert
        /// <para xml:lang="es">
        /// Instancia XmlElement a convertir
        /// </para>
        /// </param>
        /// <returns>
        /// XElement representation of XmlElement
        /// <para xml:lang="es">
        /// Representacion XElement de XmlElement
        /// </para>
        /// </returns>
        public static XElement ToXElement(this XmlElement xmlElement)
		{ return new XmlTranslator(xmlElement).CreateXElement(); }

        /// <summary>
        /// Converts the specified XmlDocument to XDocument
        /// <para xml:lang="es">
        /// Convierte el XmlDocument especificada para XDocument
        /// </para>
        /// </summary>
        /// <param name="xmlDocument">
        /// XmlDocument instance to convert
        /// <para xml:lang="es">
        /// Instancia XmlDocument a convertir
        /// </para>
        /// </param>
        /// <returns>
        /// XDocument representation of XmlDocument
        /// <para xml:lang="es">
        /// Representacion XDocument de XmlDocument
        /// </para>
        /// </returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
		{ return new XmlTranslator(xmlDocument).CreateXDocument(); }

	}
}