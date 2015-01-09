#pragma once

#include "stdafx.h"

using namespace System;

namespace EotuCore {

	public ref class GroupModel
	{
	private:
		int user_id;
		String^ name;
	public:
		property int UserId
		{
			int get() {
				return user_id;
			}

			void set(int value) {
				if (value != user_id)
				{
					user_id = value;
				}
			}
		}

		property String^ GroupName
		{
			String^ get() {
				return name;
			}

			void set(String^ value) {
				if (value != name)
				{
					name = value;
				}
			}
		}

	public:
		GroupModel(void){
			user_id = 0;
			name = String::Empty;
		}
	};
}

