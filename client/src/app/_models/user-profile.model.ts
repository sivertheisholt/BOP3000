export interface UserProfile{
    email?: string;
    username?: string;
    memberProfile?: {
        birthday?: string;
        age?: number,
        gender?: string;
        countryIso?:{
            id?: number;
            name?: string;
            twoLetterCode?: string;
            threeLetterCode?: string;
            numericCode?: string;
        },
        memberData?: {
            upvotes?: number;
            downvotes?: number;
            userDescription?: string;
            followers?: [];
            following?: [];
            userFavoriteGames?: [];
        }
    }
}