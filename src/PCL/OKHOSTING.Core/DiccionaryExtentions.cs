using System.Collections.Generic;
using System.Linq;

namespace OKHOSTING.Core
{
	public static class DiccionaryExtentions
	{
        /// <summary>
        /// Allos to perform a reverse lookup in a generic dictionary
        /// <para xml:lang="es">
        /// Permite realizar una búsqueda inversa en un diccionario genérico
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">
        /// generic type key dictionary
        /// <para xml:lang="es">
        /// Tipo generico de la llave del diccionario
        /// </para>
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Generic Value Type Dictionary
        /// <para xml:lang="es">
        /// Tipo generico del valor del diccionario
        /// </para>
        /// </typeparam>
        /// <param name="dictionary">
        /// Dictionary on applying the function to sort the dictionary in reverse order
        /// <para xml:lang="es">
        /// Diccionario sobre el que aplica la funcion para ordenar el diccionario en orden inverso
        /// </para>
        /// </param>
        /// <param name="value">
        /// value which is placed in reverse dictionary
        /// <para xml:lang="es">
        /// valor por el cual se pone el diccionario en reversa
        /// </para>
        /// </param>
        /// <returns>
        /// Returns the dictionary in reverse order
        /// <para xml:lang="es">
        /// Devuelve el diccionario en orden inverso
        /// </para>
        /// </returns>
        public static TKey Reverse<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
		{
			return dictionary.First(x => x.Value.Equals(value)).Key;
		}
	}
}