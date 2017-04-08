using System;
using NTouchTypeTrainer.Domain;
using Eto.Forms;
using System.Collections.Generic;
using NTouchTypeTrainer.Contracts.Enums;
using NTouchTypeTrainer.Contracts;
using System.IO;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer
{
	class MainClass
	{
        [STAThread]
        public static void Main(string[] args)
        {
            // ReSharper disable once ObjectCreationAsStatement
		    new Application(Eto.Platform.Detect);

            // ReSharper disable once NotAccessedVariable
            var layout = CreateAndSaveQuertzLayout();
            // ReSharper disable once RedundantAssignment
            layout = LoadLayout();
        }

	    private static KeyboardLayout CreateAndSaveQuertzLayout()
	    {
	        var layout = CreateGermanQwertz();
	        var sfd = new SaveFileDialog();
	        var dialogResult = sfd.ShowDialog(null);
	        if (dialogResult == DialogResult.Ok)
	        {
	            using (var writer = new StreamWriter(sfd.FileName))
	            {
	                writer.Write(layout.Export());
	            }
	        }
	        return layout;
	    }

	    private static KeyboardLayout LoadLayout()
	    {
            var ofd = new OpenFileDialog();
	        var dialogResult = ofd.ShowDialog(null);
	        if (dialogResult == DialogResult.Ok)
	        {
	            using (var reader = new StreamReader(ofd.FileName))
	            {
	                var exportedString = reader.ReadToEnd();
	                var layout = KeyboardLayoutImporter.Import(exportedString);

                    return layout;
	            }
	        }

	        return null;
	    }

	    private static KeyboardLayout CreateGermanQwertz()
		{
			// Sample mappings for german QWERTZ keyboard
			var hatMapping = new KeyMapping(HardwareKey.Grave, 
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('^')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('°')));
			var d1Mapping = new KeyMapping(HardwareKey.D1,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('1')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('!')));
			var d2Mapping = new KeyMapping(HardwareKey.D2,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('2')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('"')),
				Tuple.Create(Modifier.AltGr, (IMappedKey)new MappedCharacter('²')));
			var d3Mapping = new KeyMapping(HardwareKey.D3,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('3')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('§')),
				Tuple.Create(Modifier.AltGr, (IMappedKey)new MappedCharacter('³')));
			var digitsRow = new List<KeyMapping>() 
			{
				d2Mapping,
				hatMapping,
				d1Mapping,
				d3Mapping
			};

			var tabMapping = new KeyMapping(HardwareKey.Tab, new MappedUnprintable(HardwareKey.Tab));
			var qMapping = new KeyMapping(HardwareKey.Q,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('q')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('Q')),
				Tuple.Create(Modifier.AltGr, (IMappedKey)new MappedCharacter('@')));
			var upperCharacterRow = new List<KeyMapping>() 
			{
				tabMapping,
				qMapping
			};

			var capsLockMapping = new KeyMapping(HardwareKey.CapsLock, new MappedUnprintable(HardwareKey.CapsLock));
			var aMapping = new KeyMapping(HardwareKey.A,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('a')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('A')));
			var middleCharacterRow = new List<KeyMapping>() 
			{
				capsLockMapping,
				aMapping
			};

			var shiftMapping = new KeyMapping(HardwareKey.ShiftLeft, new MappedUnprintable(HardwareKey.ShiftLeft));
			var smallerMapping = new KeyMapping(HardwareKey.Backslash,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('<')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('>')),
				Tuple.Create(Modifier.AltGr, (IMappedKey)new MappedCharacter('|')));
			var yMapping = new KeyMapping(HardwareKey.Z,
				Tuple.Create(Modifier.None, (IMappedKey)new MappedCharacter('y')),
				Tuple.Create(Modifier.Shift, (IMappedKey)new MappedCharacter('Y')));
			var lowerCharacterRow = new List<KeyMapping>() 
			{
				shiftMapping,
				smallerMapping,
				yMapping,
			};

			var ctrlMapping = new KeyMapping(HardwareKey.ControlLeft, new MappedUnprintable(HardwareKey.ControlLeft));
			var superMapping = new KeyMapping(HardwareKey.SuperLeft, new MappedUnprintable(HardwareKey.SuperLeft));
			var altMapping = new KeyMapping(HardwareKey.Alt, new MappedUnprintable(HardwareKey.Alt));
			var spaceMapping = new KeyMapping(HardwareKey.Space, new MappedUnprintable(HardwareKey.Space));
			var altGrMapping = new KeyMapping(HardwareKey.AltGr, new MappedUnprintable(HardwareKey.AltGr));
			var controlkeyRow = new List<KeyMapping>() 
			{
				ctrlMapping,
				superMapping,
				altMapping,
				spaceMapping,
				altGrMapping
			};

			var layout = new KeyboardLayout(
				digitsRow.AsReadOnly(),
				upperCharacterRow.AsReadOnly(),
				middleCharacterRow.AsReadOnly(),
				lowerCharacterRow.AsReadOnly(),
				controlkeyRow.AsReadOnly());

			return layout;
		}
	}
}
