namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class GlyphLoader
    {
        private static GlyphInfo GetMathGlyphOrCreateNew(GlyphInfo[] mathGlyphInfos, ushort glyphIndex)
        {
            return mathGlyphInfos[glyphIndex] ?? (mathGlyphInfos[glyphIndex] = new GlyphInfo(glyphIndex));
        }

        public static void LoadMathGlyph(Typeface.Typeface typeface, MathTable mathTable)
        {
            //expand math info to each glyph in typeface

            typeface._mathTable = mathTable;

            //expand all information to the glyph
            int glyphCount = typeface.GlyphCount;
            GlyphInfo[] mathGlyphInfos = new GlyphInfo[glyphCount];

            int index = 0;
            //-----------------
            //2. MathGlyphInfo
            //-----------------
            {    //2.1 expand italic correction
                ItalicsCorrectionInfoTable italicCorrection = mathTable.ItalicCorrectionInfo;
                index = 0; //reset
                if (italicCorrection.CoverageTable != null)
                {
                    foreach (ushort glyphIndex in italicCorrection.CoverageTable.GetExpandedValueIter())
                    {
                        GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).ItalicCorrection = italicCorrection.ItalicCorrections[index];
                        index++;
                    }
                }
            }
            //--------
            {
                //2.2 expand top accent
                TopAccentAttachmentTable topAccentAttachment = mathTable.TopAccentAttachmentTable;
                index = 0; //reset
                if (topAccentAttachment.CoverageTable != null)
                {
                    foreach (ushort glyphIndex in topAccentAttachment.CoverageTable.GetExpandedValueIter())
                    {
                        GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).TopAccentAttachment = topAccentAttachment.TopAccentAttachment[index];
                        index++;
                    }
                }
            }
            //--------
            {
                //2.3 expand , expand shape
                index = 0; //reset
                if (mathTable._extendedShapeCoverageTable != null)
                {
                    foreach (ushort glyphIndex in mathTable._extendedShapeCoverageTable.GetExpandedValueIter())
                    {
                        GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).IsShapeExtensible = true;
                        index++;
                    }
                }
            }
            //--------
            {
                //2.4 math kern
                index = 0; //reset
                if (mathTable._mathKernInfoCoverage != null)
                {
                    KernInfoRecord[] kernRecs = mathTable._mathKernInfoRecords;
                    foreach (ushort glyphIndex in mathTable._mathKernInfoCoverage.GetExpandedValueIter())
                    {
                        GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).SetMathKerns(kernRecs[index]);
                        index++;
                    }
                }
            }
            //-----------------
            //3. MathVariants
            //-----------------
            {
                VariantsTable variants = mathTable.VariantsTable;

                //3.1  vertical
                index = 0; //reset
                foreach (ushort glyphIndex in variants.vertCoverage.GetExpandedValueIter())
                {
                    GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).VertGlyphConstruction = variants.vertConstructionTables[index];
                    index++;
                }
                //
                //3.2 horizontal
                index = 0;//reset
                if (variants.horizCoverage != null)
                {
                    foreach (ushort glyphIndex in variants.horizCoverage.GetExpandedValueIter())
                    {
                        GetMathGlyphOrCreateNew(mathGlyphInfos, glyphIndex).HoriGlyphConstruction = variants.horizConstructionTables[index];
                        index++;
                    }
                }
            }
            typeface.LoadMathGlyphInfos(mathGlyphInfos);
        }
    }
}
