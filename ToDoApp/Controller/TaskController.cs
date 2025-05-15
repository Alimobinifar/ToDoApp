using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.VMs;

namespace ToDoApp.Controller
{
    public class TaskController
    {
        #region OtherObjects
        AppDbContext db = new AppDbContext();
        #endregion

        protected virtual bool ValidateTaskData(ToDoTask task)
        {
            return task.Title.Length > 5 && task.Description.Length >= 10;
        }

        public async Task<AppResult> CreateTodoAsync(ToDoTask toDoTask)
        {
            var result = new AppResult();

            if (toDoTask == null)
            {
                result.Error = true;
                result.Msg = "Null Todo";
                return result;
            }

            if (!ValidateTaskData(toDoTask))
            {
                result.Error = true;
                result.Msg = "Validation error: Title must be more than 5 characters and Description must be at least 10.";
                return result;
            }

            try
            {
                await db.Tasks.AddAsync(toDoTask);
                await db.SaveChangesAsync();
                result.Msg = "Todo task created successfully";
            }
            catch (Exception ex)
            {
                result.Error = true;
                result.Msg = $"Database operation failed: {ex.Message}";
            }

            return result;
        }

        public async Task<AppResult> ToDoList()
        {
            AppResult result = new AppResult();
            try
            {
                var tasks = await db.Tasks.ToListAsync();
                if (tasks.Any())
                {
                    result.List = tasks;
                    
                }
                else
                {
                    result.Error = true;
                    result.Msg = "No Task found";
                }
                return result;

            }
            catch (Exception ex)
            {
                result.Msg = ex.ToString();
                result.Error = true;
            }
            return result;

        }

        public async Task<AppResult> DeleteToDoTaskAsync(int taskId)
        {
            var result = new AppResult();

            try
            {
                var task = await db.Tasks.FindAsync(taskId);

                if (task == null)
                {
                    result.Error = true;
                    result.Msg = "Task not found.";
                    return result;
                }

                db.Tasks.Remove(task);
                await db.SaveChangesAsync();

                result.Msg = "Todo task deleted successfully.";
            }
            catch (Exception ex)
            {
                result.Error = true;
                result.Msg = $"Error deleting task: {ex.Message}";
            }

            return result;
        }

        public async Task<AppResult> DoneTaskAsync(int taskId)
        {
            var result = new AppResult();

            try
            {
                var task = await db.Tasks.FindAsync(taskId);

                if (task == null)
                {
                    result.Error = true;
                    result.Msg = "Task not found.";
                    return result;
                }

                // Mark task as done
                task.IsDone = true;

                await db.SaveChangesAsync();

                result.Msg = "Todo task marked as done successfully.";
            }
            catch (Exception ex)
            {
                result.Error = true;
                result.Msg = $"Error updating task status: {ex.Message}";
            }

            return result;
        }

        public async Task<AppResult> UpdateTaskAsync(int taskId, ToDoTask updatedTask)
        {
            var result = new AppResult();
            try
            {
                var task = await db.Tasks.FindAsync(taskId);
                if (task == null)
                {
                    result.Error = true;
                    result.Msg = "This task not found";
                    return result;
                }
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.IsDone = updatedTask.IsDone;
                await db.SaveChangesAsync();
                result.Msg = "Task updated successfully.";
            }
            catch (Exception ex)
            {
                result.Msg = $"Update failed: {ex.Message}";
                result.Error = true;
            }
            return result;
        }
    }
}
