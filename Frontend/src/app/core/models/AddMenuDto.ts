import { MenuDishDto } from "./MenuDishDto";
import { OrderInfoDto } from "./OrderInfoDto";

export interface AddMenuDto {
    Day: string;
    Dishes: MenuDishDto[];
    Sides: string[];
    SpecialOffer: string;
    OrderInfo: OrderInfoDto;
}