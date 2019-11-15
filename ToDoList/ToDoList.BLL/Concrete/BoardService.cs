using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Abstract;
using ToDoList.CustomException;
using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete.EntityFramework.DAL;
using ToDoList.Model;

namespace ToDoList.BLL.Concrete
{
    public class BoardService : IBoardService
    {
        EFBoardDAL _boardDAL;

        public BoardService()
        {
            _boardDAL = new EFBoardDAL();
        }

        public bool Delete(Board model)
        {
            CheckAvailableBoard(model.ID);
            return _boardDAL.Delete(model) > 0;
        }

        public bool Delete(int entityID)
        {
            return _boardDAL.Delete(Get(entityID)) > 0;
        }

        public Board Get(int entityID)
        {
            try
            {
                return _boardDAL.Get(a => a.ID == entityID);
            }
            catch
            {
                throw new ModelAvailableException("Board");
            }
        }

        public List<Board> GetAll()
        {
            try
            {
                return _boardDAL.GetAll().ToList();
            }
            catch
            {
                throw new EmptyListException("Board");
            }
        }

        public List<Board> GetBoardsByUser(User user)
        {
            try
            {
                return _boardDAL.GetAll(a => a.UserID == user.ID).ToList();
            }
            catch
            {
                throw new EmptyListException("Board");
            }
        }

        public bool Insert(Board model)
        {
            CheckNullFields(model);
            CheckMaxLengthName(model.Name);
            return _boardDAL.Add(model) > 0;
        }

        public bool Update(Board model)
        {
            CheckNullFields(model);
            CheckAvailableBoard(model.ID);
            CheckMaxLengthName(model.Name);
            return _boardDAL.Update(model) > 0;
        }


        void CheckAvailableBoard(int boardID)
        {
            bool flag = false;

            foreach (Board item in GetAll())
            {
                if (item.ID == boardID)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("Board");
            }
        }

        void CheckMaxLengthName(string name)
        {
            if (name.Length > 75)
            {
                throw new MaxLengthException("Name", 75);
            }
        }

        void CheckNullFields(Board board)
        {
            if (string.IsNullOrWhiteSpace(board.Name))
            {
                throw new NotNullException("Name");
            }
        }
    }
}
