using ApiApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.ModelsDTO.OrderDTO
{
    public class OrderToHandleDto
    {
        [Required(ErrorMessage = "Не указано имя,фамилия")]
        public string FullName { get; set; }        // имя фамилия покупателя
        [Required(ErrorMessage = "Не указан адрес")]
        public string Address { get; set; } // адрес покупателя
        [Required(ErrorMessage = "Не указан телефон")]
        [MaxLength(15)]
        public string ContactPhone { get; set; } // контактный телефон покупателя
        [MinLength(1, ErrorMessage = "At least one Something is required")]
        public List<Guid> PhonesIds { get; set; } = new List<Guid>();

    }
}
