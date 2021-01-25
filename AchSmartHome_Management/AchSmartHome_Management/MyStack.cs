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
    /// <summary>
    /// Мой класс стека, основанный на списке
    /// Отличие от обычного Stack<T> в том,
    /// что в моём классе есть функции как в JavaScript:
    /// <code>Shift();</code> = удалить первый элемент стека.
    /// <code>Unshift(T item);</code> = добавить элемент в начало стека.
    /// </summary>
    /// <typeparam name="T">Тип данных в стеке</typeparam>
    class MyStack<T> : List<T>
    {
        /// <summary>
        /// Мой класс стека, основанный на списке
        /// Отличие от обычного Stack<T> в том,
        /// что в моём классе есть функции как в JavaScript:
        /// <code>Shift();</code> = удалить первый элемент стека.
        /// <code>Unshift(T item);</code> = добавить элемент в начало стека.
        /// </summary>
        public MyStack()
        {
            new List<T>();
        }

        /// <summary>
        /// Получить последний элемент стека
        /// </summary>
        /// <returns>Последний элемент стека</returns>
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

        /// <summary>
        /// Удалить последний элемент стека
        /// </summary>
        public void Pop()
        {
            if (this.Count < 1)
                throw new InvalidOperationException("There is no items in stack!");
            this.RemoveAt(this.Count - 1);
        }
        /// <summary>
        /// Удалить первый элемент стека
        /// </summary>
        public void Shift()
        {
            if (this.Count < 1)
                throw new InvalidOperationException("There is no items in stack!");
            this.RemoveAt(0);
        }

        /// <summary>
        /// Добавить элемент в конец стека (FIFO)
        /// </summary>
        /// <param name="item">Элемент для добавления</param>
        public void Push(T item)
        {
            this.Add(item);
        }
        /// <summary>
        /// Добавить элемент в начало стека
        /// </summary>
        /// <param name="item">Элемент для добавления</param>
        public void Unshift(T item)
        {
            /*
            for (int i = (this.Count - 1); i > -1; i--)
            {
                this.Insert(i + 1, this[i]);
            }
            */
            this.Insert(0, item);
        }
    }
}
