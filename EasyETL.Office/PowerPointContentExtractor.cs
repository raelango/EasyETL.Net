using DocumentFormat.OpenXml.Packaging;
using EasyETL.Extractors;
using System;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Office
{
    [DisplayName("Powerpoint Extractor")]
    public class PowerPointContentExtractor : AbstractContentExtractor
    {
        public override Stream GetStream(Stream inStream)
        {
            string content = ReadPowerPointText(PresentationDocument.Open(inStream, false));
            return new MemoryStream(Encoding.Default.GetBytes(content));
        }

        /// <summary> 
        ///  Read Word Document 
        /// </summary> 
        /// <returns>Plain Text in document </returns> 
        public string ReadPowerPointText(PresentationDocument package)
        {
            int numSlides = CountSlides(package);
            StringBuilder sb = new StringBuilder();
            for (int slideIndex = 0; slideIndex < numSlides; slideIndex ++)
            {
                string slideContents = GetSlideIdAndText(package, slideIndex);
                sb.AppendLine(slideContents);
            }
            return sb.ToString();
        }

        // Count the slides in the presentation.
        public int CountSlides(PresentationDocument presentationDocument)
        {
            // Check for a null document object.
            if (presentationDocument == null)
            {
                throw new ArgumentNullException("presentationDocument");
            }

            int slidesCount = 0;

            // Get the presentation part of document.
            PresentationPart presentationPart = presentationDocument.PresentationPart;
            // Get the slide count from the SlideParts.
            if (presentationPart != null) slidesCount = presentationPart.SlideParts.Count();
            // Return the slide count to the previous method.
            return slidesCount;
        }

        public string GetSlideIdAndText(PresentationDocument ppt, int index)
        {
            // Get the relationship ID of the first slide.
            PresentationPart part = ppt.PresentationPart;
            OpenXmlElementList slideIds = part.Presentation.SlideIdList.ChildElements;

            string relId = (slideIds[index] as SlideId).RelationshipId;

            // Get the slide part from the relationship ID.
            SlidePart slide = (SlidePart)part.GetPartById(relId);

            // Build a StringBuilder object.
            StringBuilder paragraphText = new StringBuilder();

            // Get the inner text of the slide:
            IEnumerable<A.Text> texts = slide.Slide.Descendants<A.Text>();
            foreach (A.Text text in texts)
            {
                paragraphText.AppendLine(text.Text);
            }
            return paragraphText.ToString();
        }

    }

}
