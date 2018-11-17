using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS
{
    public class ContentPack
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public string CPVersion { get; private set; }

        private List<ContentPackElement> elementsList = new List<ContentPackElement>();

        public List<ContentPackElement> Elements
        {
            get
            {
                return elementsList;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="contentPackData">Content pack data.</param>
        public ContentPack(ContentPackData contentPackData)
        {
            this.Name = contentPackData.Name;
            this.Id = contentPackData.Id;
            this.Author = contentPackData.Author;
            this.Version = contentPackData.Version;
            this.CPVersion = contentPackData.CPVersion;

            CreateElements(contentPackData);
        }

        /// <summary>
        /// Returns List of Content Pack Elements with specific type.
        /// </summary>
        /// <param name="type">String representing type name.</param>
        /// <returns></returns>
        public List<ContentPackElement> GetElementsOfType(string type)
        {
            List<ContentPackElement> outputList = new List<ContentPackElement>();

            foreach (ContentPackElement element in elementsList)
            {
                if (element.Type == type)
                    outputList.Add(element);
            }

            return outputList;
        }

        /// <summary>
        /// Returns Content Pack Element with specific ID. If there's none throws Exception.
        /// </summary>
        /// <param name="id">String representing id.</param>
        /// <returns></returns>
        public ContentPackElement GetElementWithId(string id)
        {
            foreach (ContentPackElement element in elementsList)
            {
                if (element.Id == id)
                    return element;
            }

            throw new Exception("No element with id: " + id + " found.");
        }

        private void CreateElements(ContentPackData contentPackData)
        {
            foreach (ContentPackElementData elementData in contentPackData.Elements)
            {
                ContentPackElement element = new ContentPackElement(elementData, this);
                elementsList.Add(element);
            }
        }
    }
}
