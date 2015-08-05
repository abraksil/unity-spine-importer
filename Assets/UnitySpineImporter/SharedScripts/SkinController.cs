using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnitySpineImporter;





namespace UnitySpineImporter{
	public class SkinController : MonoBehaviour {
		public Skin defaultSkin;		
		public Skin[]   skins;
		public Slot[] 	slots;
		public GameObject[] bones;
		public string[] bonesName;

		public int      activeSkinId;


		Skin[]        _allSkins;
		public Skin[] allSkins{
			get{
				if (_allSkins == null){
					if (defaultSkin != null && defaultSkin.slots !=null && defaultSkin.slots.Length > 0){
						_allSkins = new Skin[skins.Length+1];
						Array.Copy(skins,_allSkins,skins.Length);
						_allSkins[_allSkins.Length -1] = defaultSkin;
					} else {
						_allSkins = skins;
					}
				}
				return _allSkins;
			}
		}		

		public GameObject getBoneByName( string name ) {
			for( int i = 0; i!=bonesName.Length; i++ ) {
				if ( bonesName[ i ] == name ) return bones[ i ];
			}

			return null;
		}

		public void deactivateAllAttachments(){
			foreach(Skin skin in allSkins){
				foreach(SkinSlot slot in skin.slots){
					foreach(SkinSlotAttachment attachment in slot.attachments){
						attachment.gameObject.SetActive(false);
					}
				}
			}
		}		

		public void showDefaulSlots(){
			deactivateAllAttachments();

			if (skins.Length > 0){
				activeSkinId = 0;			
				setSkin(activeSkinId);
			} else {
				activeSkinId = -1;
			}

			foreach (Slot slot in slots){
				slot.showDefaultAttachment();
			}
		}

		private Dictionary< string, int >		mSlotToSkinExceptions		=		new Dictionary< string, int >();

		public int getSkinId( string skinName ) {
			int res = -1;
			for ( int i=0; i != skins.Length; i++ ) {
				if ( skins[ i ].name == skinName ) {
					res = i;
					break;
				}
			}

			return res;
		}

		public SkinSlot getSkinOnSlot( string slotName ) {
			int skinsId = -1;
			if ( mSlotToSkinExceptions.ContainsKey( slotName ) ) skinsId = mSlotToSkinExceptions[ slotName ];
			else skinsId = activeSkinId;
			return getSkinSlot( slotName, skinsId );
		}

		public void setSkin(int skinId ){
			foreach ( KeyValuePair< string, int > p in mSlotToSkinExceptions ) {
				setActive( p.Key, p.Value, false );
			}

			mSlotToSkinExceptions.Clear( );

			skins[activeSkinId].setActive(false);
			skins[skinId].setActive(true);
			activeSkinId = skinId;
		}

		public void setSkinExceptionForSlot( string slotName, int skinId ) {
			if ( skinId < 0 || skinId >= skins.Length ) Debug.LogError( "Skin Id out of range: " + skinId );
			if ( mSlotToSkinExceptions.ContainsKey( slotName ) ) {
				setActive( slotName, mSlotToSkinExceptions[ slotName ], false );
			} else {
				setActive( slotName, activeSkinId, false );
			}

			mSlotToSkinExceptions[ slotName ] = skinId;
			setActive( slotName, skinId, true );
		}

		private SkinSlot 
			getSkinSlot( string slotName, int skinId ) {
			if ( skinId < 0 || skinId >= skins.Length ) Debug.LogError( "Skin Id out of range: " + skinId );
			Skin skin = skins[ skinId ];
			for ( int i=0; i != skin.slots.Length; i++ ) {
				if ( skin.slots[ i ].name == slotName ) return skin.slots[ i ];
			}

			for ( int i=0; i != defaultSkin.slots.Length; i++ ) {
				if ( defaultSkin.slots[ i ].name == slotName ) return defaultSkin.slots[ i ];
			}

			return null;
		}

		private void setActive( string slotName, int skinId, bool on ) {
			SkinSlot skinslot = getSkinSlot( slotName, skinId );
			if ( skinslot != null ) {
				for ( int i = 0; i != skinslot.attachments.Length; i++ ) {
					skinslot.attachments[ i ].gameObject.SetActive( on );
				}
			}
		}
	}
}