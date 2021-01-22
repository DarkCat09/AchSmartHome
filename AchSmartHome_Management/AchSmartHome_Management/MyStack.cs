/*
 * Copyright © 2020 Чечкенёв Андрей
 * 
 * This file is part of AchSmartHome.
 * 
 * AchSmartHome is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * AchSmartHome is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with AchSmartHome.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;

namespace AchSmartHome_Management
{
    class MyStack<T> : List<T>
    {
        public MyStack()
        {
            new List<T>();
        }

        public T Peek()
        {
            if (this.Count < 1)
                throw new InvalidOperationException("There is no items in stack!");
            T lastItem = this[0];
            try
            {
                lastItem = this[this.Count - 1];
            }
            catch (IndexOutOfRangeException) {}
            return lastItem;
        }

        public void Pop()
        {
            if (this.Count < 1)
                throw new InvalidOperationException("There is no items in stack!");
            this.RemoveAt(this.Count - 1);
        }
        public void Shift()
        {
            if (this.Count < 1)
                throw new InvalidOperationException("There is no items in stack!");
            this.RemoveAt(0);
        }

        public void Push(T item)
        {
            this.Add(item);
        }
        public void Unshift(T item)
        {
            for (int i = (this.Count - 1); i > -1; i--)
            {
                this.Insert(i + 1, this[i]);
            }
            this.Insert(0, item);
        }
    }
}
