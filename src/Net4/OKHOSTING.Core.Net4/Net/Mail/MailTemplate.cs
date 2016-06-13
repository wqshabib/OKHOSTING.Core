using System;
using System.Reflection;
using System.Net.Mail;
using System.IO;

namespace OKHOSTING.Core.Net4.Net.Mail
{

    /// <summary>
    /// A template for emails. Child classes can contain fields and 
    /// properties that will replace tags in the message body
    /// <para xml:lang="es">
    /// Una plantilla para mensajes de correo electrónico.las clases
    /// hijas pueden contener campos y propiedades, que sustituirán a las etiquetas en el cuerpo del mensaje
    /// </para>
    /// </summary>
    public abstract class MailTemplate : MailMessage
	{

        #region Properties

        /// <summary>
        /// Returns the Mail template directory for the application
        /// <para xml:lang="es">
        /// Devuelve el directorio de plantillas de correo para la aplicación
        /// </para>
        /// </summary>
        public static string MailTemplatesDirectory
		{ 
			get 
			{ 
				return AppDomain.CurrentDomain.BaseDirectory + @"Custom\MailTemplates\"; 
			} 
		}

        /// <summary>
        /// Returns the mail template file for the mail template type
        /// <para xml:lang="es">
        /// Devuelve el archivo de plantilla de correo para el tipo de plantilla de correo
        /// </para>
        /// </summary>
        public string TemplateFile
		{ 
			get 
			{ 
				return MailTemplate.MailTemplatesDirectory + this.GetType().FullName + ".html"; 
			} 
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance
        /// <para xml:lang="es">
        /// Crea una nueva instancia
        /// </para>
		/// </summary>
		public MailTemplate()
		{
			//Creating the MailTemplates subdirectory (if applies)
			if (!Directory.Exists(MailTemplate.MailTemplatesDirectory)) Directory.CreateDirectory(MailTemplate.MailTemplatesDirectory);

			//Validating if template exists
			if (!File.Exists(this.TemplateFile))
			{
				throw new InvalidOperationException("The mail template type '" + this.GetType().FullName + "' dont have it's physical template on '" + this.TemplateFile + "'");
			}

			//Reading the mail body
			this.Body = File.ReadAllText(this.TemplateFile);

			//Establishing that the mail have a Html mail body
			this.IsBodyHtml = true;
		}

        #endregion

        #region Implementation

        /// <summary>
        /// Replace all tags in the Subject and Body with the specified value
        /// <para xml:lang="es">
        /// Vuelva a colocar todas las etiquetas en el asunto y el cuerpo con el valor especificado
        /// </para>
        /// </summary>
        /// <param name="tagName">
        /// Tag that will be replaced in the form of &lt;@tagname&gt;
        /// <para xml:lang="es">
        /// Etiqueta que será reemplazado en forma de & lt; nombre de etiqueta @ & gt;
        /// </para>
        /// </param>
        /// <param name="value">Value that will replace the tag</param>
        public void ReplaceTag(string tagName, string value)
		{
			//Replacing custom tags on mail subject
			this.Subject = this.Subject.Replace("<@" + tagName + ">", value);
			this.Subject = this.Subject.Replace("&lt;@" + tagName + "&gt;", value);

			//Replacing custom tags on mail body
			this.Body = this.Body.Replace("<@" + tagName + ">", value);
			this.Body = this.Body.Replace("&lt;@" + tagName + "&gt;", value);
		}

        /// <summary>
        /// Replace all tags in the subject and body, and prepares the message to be sent
        /// <para xml:lang="es">
        /// Vuelva a colocar todas las etiquetas en el asunto y el cuerpo con el valor especificado
        /// </para>
        /// </summary>
        /// <remarks>
        /// Child classes must override this method to replace all tags
        /// <para xml:lang="es">
        /// clases hijas deben reemplazar este método para reemplazar todas las etiquetas
        /// </para>
        /// </remarks>
        public virtual void Init()
		{
			//Getting reference to MailTemplate Type 
			Type MailTemplateType = this.GetType();

			//Crossing MailTemplate Type fields
			foreach (FieldInfo field in MailTemplateType.GetFields())
			{
				//Validating if the current field is not static
				if (!field.IsStatic && field.DeclaringType != typeof(MailTemplate))
				{
					//Loading the value for field on this object
					object fieldValue = field.GetValue(this);

					//Gettig the value on string
					string stringFieldValue;
					if (fieldValue == null)
					{
						stringFieldValue = string.Empty;
					}
					else
					{
						//Getting reference to ToString() method
						stringFieldValue = fieldValue.ToString();
					}

					//Replacing value on template
					this.ReplaceTag(field.Name, stringFieldValue);
				}
			}

			//Crossing MailTemplate Type properties
			foreach (PropertyInfo property in MailTemplateType.GetProperties())
			{
				//Validating if the current property is not static
				if (!property.GetGetMethod().IsStatic && property.DeclaringType != typeof(MailTemplate))
				{
					//Loading the value for property on this object
					object propertyValue = property.GetValue(this, null);

					//Gettig the value on string
					string stringPropertyValue;
					if (propertyValue == null)
					{
						stringPropertyValue = string.Empty;
					}
					else
					{
						//Getting reference to ToString() method
						stringPropertyValue = propertyValue.ToString();
					}

					//Replacing value on template
					this.ReplaceTag(property.Name, stringPropertyValue);
				}
			}
		}

		#endregion
	}
}