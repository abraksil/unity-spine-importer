using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnitySpineImporter {
	public class SlotColorControler : MonoBehaviour {
		public Color					Color			=		Color.white;
		public SpriteRenderer[]			Sprites;

		private float					mAlfaControl	=		1.0f;
		private Color					LastColor		=		Color.white;
		private List< SpriteRenderer >	spriteList		=		null;

		private void updateColor ( ) {
			LastColor = Color;
			Color col = Color;
			col.a = col.a * mAlfaControl;
			for ( int i = 0; i != Sprites.Length; i++ ) {
				Sprites[ i ].color = col;
			}
		}

		void Update( ) {
			if ( LastColor != Color ) updateColor( );
		}

		public void stor ( SpriteRenderer sprite ) {
			if ( spriteList == null ) spriteList = new List< SpriteRenderer > ( );

			spriteList.Add( sprite );
		}

		public void init( ) {
			if ( spriteList != null ) {
				Sprites = spriteList.ToArray( );
				spriteList = null;
			}
		}

		public void setAlfa( float newAlfa ) {
			if ( newAlfa != mAlfaControl ) {
				mAlfaControl = newAlfa;
				updateColor( );
			}
		}
	}
}
