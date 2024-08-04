namespace FontParser.Tables.AdvancedLayout.Ligatures
{
    //A Caret Values table (CaretValues), which defines caret positions for a ligature,
    //can be any of three possible formats.
    //One format uses design units to define the caret position.
    //The other two formats use a contour point or (in non-variable fonts) a Device table to fine-tune a caret's position at specific font sizes
    //and device resolutions.
    //In a variable font, the third format uses a VariationIndex table (a variant of a Device table)
    //to reference variation data for adjustment of the caret position for the current variation instance, as needed.
    //Caret coordinates are either X or Y values, depending upon the text direction.

    /// <summary>
    /// A Caret Values table (CaretValues)
    /// </summary>
    public class CaretValues
    {
    }
}
