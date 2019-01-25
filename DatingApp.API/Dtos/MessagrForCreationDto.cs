using System;

namespace DatingApp.API.Dtos {
    public class MessagrForCreationDto {

        public int SenderId { get; set; }

        public int RecipientId { get; set; }

        public DateTime MessageSent { get; set; }

        public string Content { get; set; }

        public MessagrForCreationDto () {
            MessageSent = DateTime.Now;
        }
    }
}