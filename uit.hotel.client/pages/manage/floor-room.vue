<template>
    <div>
        <block-flex->
            <b-button
                class="m-2"
                variant="white"
                @click="$refs.floor_add.open()"
            >
                <icon- class="mr-1" i="plus" />
                <span>Thêm tầng mới</span>
            </b-button>
            <b-button
                class="m-2 ml-auto"
                variant="white"
                @click="showInactive = !showInactive"
            >
                <icon- :i="showInactive ? 'eye' : 'eye-off'" class="mx-1" />
                <span>
                    {{
                        `Đang ${
                            showInactive ? 'hiện' : 'ẩn'
                        } phòng và tầng đã bị vô hiệu hóa`
                    }}
                </span>
            </b-button>
        </block-flex->
        <query-
            v-slot="{ data: { floors } }"
            :query="getFloors"
            class="hotel-map row flex-1 overflow-hidden"
            child-class="col m-2 p-3 bg-white rounded shadow-sm overflow-auto"
        >
            <div>
                <table>
                    <tr v-for="floor in floorsFilter(floors)" :key="floor.id">
                        <td>
                            <b-button
                                :variant="floor.isActive ? 'main' : 'gray'"
                                @contextmenu.prevent="
                                    $refs.context_floor.open($event, { floor })
                                "
                            >
                                Tầng {{ floor.name }}
                            </b-button>
                        </td>
                        <td
                            v-for="room in roomsFilter(floor.rooms)"
                            :key="room.id"
                        >
                            <b-button
                                class="position-relative"
                                :variant="room.isActive ? 'light-blue' : 'gray'"
                                @contextmenu.prevent="
                                    $refs.context_room.open($event, {
                                        room,
                                        floor,
                                    })
                                "
                            >
                                Phòng {{ room.name }}
                                <br />
                                {{ room.roomKind.name }}
                                <icon-
                                    v-if="!room.isClean"
                                    i="broom"
                                    class="position-absolute b-0 r-0 m-1"
                                />
                            </b-button>
                        </td>
                    </tr>
                </table>
            </div>
        </query->
        <context-manage-room- ref="context_room" />
        <context-manage-floor- ref="context_floor" />
        <popup-floor-add- ref="floor_add" />
        <popup-floor-update- ref="floor_update" />
        <popup-room-add- ref="room_add" />
        <popup-room-detail- ref="room_detail" />
        <popup-room-update- ref="room_update" />
        <popup-room-kind-detail- ref="room_kind_detail" />
    </div>
</template>
<script lang="ts">
import { Component, mixins } from 'nuxt-property-decorator';
import { getFloors } from '~/graphql/documents';
import { DataMixin, Page } from '~/components/mixins';
import { GetFloors } from '~/graphql/types';

@Component({
    name: 'floor-room-',
})
export default class extends mixins<Page, {}>(Page, DataMixin({ getFloors })) {
    head() {
        return {
            title: 'Sơ đồ khách sạn',
        };
    }

    floorsFilter(floors: GetFloors.Floors[]): GetFloors.Floors[] {
        if (this.showInactive) return floors;

        return floors
            .filter(f => f.isActive)
            .sort((a, b) => (a.name > b.name ? 1 : -1));
    }

    roomsFilter(rooms: GetFloors.Rooms[]): GetFloors.Rooms[] {
        if (this.showInactive) return rooms;

        return rooms
            .filter(r => r.isActive)
            .sort((a, b) => (a.name > b.name ? 1 : -1));
    }

    showInactive = false;
}
</script>
