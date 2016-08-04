//Copyright (c) 2014-2015 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
//Licensed under the MIT License - https://raw.github.com/Robmaister/SharpFont.HarfBuzz/master/LICENSE

using System;
using System.Runtime.InteropServices;

namespace SharpFont.HarfBuzz
{
	public class Font
	{
		#region Members
		private IntPtr reference;
		#endregion

		#region Constructors
		public static Font FromFTFace(Face face)
		{
			return new Font { reference = HB.hb_ft_font_create(face.Reference, IntPtr.Zero) };
		}
		#endregion

		#region Properties
		internal IntPtr Reference { get { return reference; } }
        #endregion

        #region Methods
        public int[] GetLigatureCarets(Direction direction, uint glyph)
        {
            var len = HB.hb_ot_layout_get_ligature_carets(reference, direction, glyph, 0, IntPtr.Zero, IntPtr.Zero);
            var buf = Marshal.AllocHGlobal((int)(len * 4));
            var ret = new int[len];
            var p = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(p, (int)len);
            HB.hb_ot_layout_get_ligature_carets(reference, direction, glyph, 0, p, buf);
            Marshal.FreeHGlobal(p);
            Marshal.Copy(buf, ret, 0, (int)(len * 4));
            Marshal.FreeHGlobal(buf);
            return ret;
        }
        public Tuple<int, int> GetGlyphKerningForDirection(uint firstGlyph, uint secondGlyph, Direction direction)
        {
            int x, y;
            HB.hb_font_get_glyph_kerning_for_direction(reference, firstGlyph, secondGlyph, direction, out x, out y);
            return new Tuple<int, int>(x, y);
        }
		#endregion
	}
}
